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
                var mapping = mappings.SingleOrDefault(x => x.PodioFieldName == field.Label);
                if (mapping == null)
                    continue;

                switch (mapping.FieldType)
                {
                    case FieldType.Image:
                        var images = item.Field<ImageItemField>(field.ExternalId)?.Images.ToList();
                        foreach (var file in images)
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
                        var userField = (ContactItemField)field;
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
                        var categoryField = item.Field<CategoryItemField>(field.ExternalId);
                        var categoryValue = categoryField.Values.First().Value<string>("text");
                        var mappedValue = mapping.CategoryMappings.SingleOrDefault(x => x.PodioValue == categoryValue)?.AzureValue;
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
                        var numberField = (NumericItemField)field;
                        var numberValue = numberField.Value.HasValue ? numberField.Value.Value.ToString() : "0.0";
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
                        var stringField = (TextItemField)field;
                        patchDocument.Add(
                            new JsonPatchOperation()
                            {
                                Operation = Operation.Add,
                                Path = $"/fields/{mapping.AzureFieldName}",
                                Value = stringField.Value
                            }
                        );
                        break;
                    case FieldType.Date:
                        var dateField = (DateItemField)field;
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
