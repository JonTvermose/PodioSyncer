using Microsoft.EntityFrameworkCore;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using PodioAPI.Models;
using PodioAPI.Utils.ItemFields;
using PodioSyncer.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using PodioSyncer.Data.Models;
using PodioAPI.Utils.ApplicationFields;
using PodioSyncer.Models.Podio;
using Newtonsoft.Json.Linq;

namespace PodioSyncer.Extensions
{
    public static class PodioItemExtensions
    {
        public static async Task<JsonPatchDocument> GetChangesAsync(this Item item, QueryDb queryDb, WorkItemTrackingHttpClient itemClient, PodioAPI.Podio podio)
        {
            var patchDocument = new JsonPatchDocument();

            var appId = item.App.AppId;
            var mappings = queryDb.FieldMappings.Include(x => x.CategoryMappings).ToList();
            foreach (var field in item.Fields)
            {
                var mapping = mappings.SingleOrDefault(x => x.PodioFieldName == field.ExternalId);
                if (mapping == null)
                    continue;

                switch (mapping.FieldType)
                {
                    case FieldType.Image:
                        foreach (var file in item.Field<ImageItemField>(field.ExternalId)?.Images.ToList())
                        {
                            FileResponse fileResponse = await podio.FileService.DownloadFile(file);

                            using (var stream = new MemoryStream(fileResponse.FileContents))
                            {
                                var imgReference = await itemClient.CreateAttachmentAsync(stream);

                                patchDocument.Add(
                                    new JsonPatchOperation()
                                    {
                                        Operation = Operation.Add,
                                        Path = "/relations/-",
                                        Value = new
                                        {
                                            rel = "AttachedFile",
                                            url = imgReference.Url,
                                            attributes = new { comment = file.Name }
                                        }
                                    }
                                );
                            }
                        }
                        break;
                    case FieldType.File:
                        break;
                    case FieldType.User:
                        var userField = item.Field<ContactItemField>(field.ExternalId);
                        var contact = userField.Contacts.FirstOrDefault();
                        string commentText = "";
                        if(queryDb.Links.Any(x => x.PodioId == item.ItemId))
                        {
                            commentText = $"Updated by {contact.Name}";
                        } else
                        {
                            commentText = $"Created by {contact.Name}";
                        }
                        patchDocument.Add(
                            new JsonPatchOperation()
                            {
                                Operation = Operation.Add,
                                Path = $"/fields/System.History",
                                Value = commentText
                            }
                        );
                        break;
                    case FieldType.Category:                        
                        var categoryValue = field.Values.ToObject<PodioValue[]>();
                        var mappedValue = mapping.CategoryMappings.SingleOrDefault(x => x.PodioValue == categoryValue[0].Value.Text)?.AzureValue;
                        patchDocument.Add(
                            new JsonPatchOperation()
                            {
                                Operation = Operation.Add,
                                Path = $"/fields/{mapping.AzureFieldName}",
                                Value = mappedValue
                            }
                        );
                        break;
                    case FieldType.Boolean:
                        break;
                    case FieldType.Int:
                        var numberValue = field.Values.First().Value<string>("value");
                        patchDocument.Add(
                            new JsonPatchOperation()
                            {
                                Operation = Operation.Add,
                                Path = $"/fields/{mapping.AzureFieldName}",
                                Value = numberValue
                            }
                        );
                        break;
                    case FieldType.String:
                        var stringValue = field.Values.First().Value<string>("value");
                        patchDocument.Add(
                            new JsonPatchOperation()
                            {
                                Operation = Operation.Add,
                                Path = $"/fields/{mapping.AzureFieldName}",
                                Value = stringValue
                            }
                        );
                        break;
                    case FieldType.Date:
                        var dateField = item.Field<DateItemField>(field.ExternalId);
                        var dateValue = dateField.Start.HasValue ? dateField.Start.Value : dateField.End.HasValue ? dateField.End.Value : DateTime.UtcNow;
                        patchDocument.Add(
                            new JsonPatchOperation()
                            {
                                Operation = Operation.Add,
                                Path = $"/fields/{mapping.AzureFieldName}",
                                Value = dateValue
                            }
                        );
                        break;
                }

            }
            return patchDocument;
        }
    }
}
