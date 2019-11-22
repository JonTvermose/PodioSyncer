using Microsoft.EntityFrameworkCore;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using PodioAPI.Models;
using PodioAPI.Utils.ItemFields;
using PodioSyncer.Data;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PodioSyncer.Data.Models;
using PodioSyncer.Models.Podio;

namespace PodioSyncer.Extensions
{
    public enum AzureType
    {
        Bug,
        UserStory,
        Task,
        ProductBacklogItem
    }

    public static class PodioItemExtensions
    {
        public static string ToAzureTypeString(this AzureType azureType)
        {
            switch (azureType)
            {
                case AzureType.Bug:
                    return "Bug";
                case AzureType.UserStory:
                    return "User Story";
                case AzureType.Task:
                    return "Task";
                case AzureType.ProductBacklogItem:
                    return "Product Backlog Item";
            }
            return "";
        }

        public static string GetPodioType(this Item item, string podioTypeExternalId)
        {
            var field = item.Field<ItemField>(podioTypeExternalId);
            return field.Values.ToObject<PodioValue[]>().First().Value.Text;
        }

        public static string GetAzureType(this Item item, PodioApp app)
        {
            switch (item.GetPodioType(app.PodioTypeExternalId).ToLower())
            {
                case "bug":
                    return AzureType.Bug.ToAzureTypeString();
                case "improvement":
                    return AzureType.UserStory.ToAzureTypeString();
                case "inquiry":
                    return null;
            }
            return null;
        }

        public static async Task<JsonPatchDocument> GetChangesAsync(this Item item, QueryDb queryDb, WorkItemTrackingHttpClient itemClient, PodioAPI.Podio podio, PodioApp app, bool ignoreRequirements)
        {
            var patchDocument = new JsonPatchDocument();
            var podioType = item.GetPodioType(app.PodioTypeExternalId);
            var appId = item.App.AppId;

            var mappings = queryDb.TypeMappings
                .Include(x => x.FieldMappings)
                .ThenInclude(x => x.CategoryMappings)
                .Where(x => x.PodioType == podioType)
                .SelectMany(x => x.FieldMappings)
                .ToList();

            if (ignoreRequirements)
            {
                foreach (var categoryField in mappings.Where(x => x.FieldType == FieldType.Category))
                {
                    var requiredValue = categoryField.CategoryMappings.SingleOrDefault(x => x.Required);
                    if (requiredValue != null)
                    {
                        var field = item.Fields
                            .SingleOrDefault(x => x.ExternalId == categoryField.PodioFieldName);
                        if(field == null)
                        {
                            throw new ArgumentException("Rules dictate this item should not be synced");
                        }
                        var podioValueField = field.Values.ToObject<PodioValue[]>();
                        var fieldValue = podioValueField.FirstOrDefault()?.Value?.Text;
                        if (!requiredValue.PodioValue.Equals(fieldValue, StringComparison.InvariantCultureIgnoreCase))
                        {
                            throw new ArgumentException("Rules dictate this item should not be synced");
                        }
                    }
                }
            }

            foreach (var field in item.Fields)
            {
                var mapping = mappings.SingleOrDefault(x => x.PodioFieldName == field.ExternalId);
                if (mapping == null || (string.IsNullOrWhiteSpace(mapping.AzureFieldName) && !(mapping.FieldType == FieldType.Image || mapping.FieldType == FieldType.User)))
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
                                            attributes = new { name = file.Name }
                                        }
                                    }
                                );
                            }
                        }
                        break;
                    case FieldType.User:
                        var userField = item.Field<ContactItemField>(field.ExternalId);
                        var contact = userField.Contacts.FirstOrDefault();
                        string commentText = "";
                        if(!queryDb.Links.Any(x => x.PodioId == item.ItemId))
                        {
                            commentText = $"Created by {contact.Name}</br>Podio url: <a href=\"{item.Link}\" target=\"_blank\">{item.Link}</a>";
                            patchDocument.Add(
                                new JsonPatchOperation()
                                {
                                    Operation = Operation.Add,
                                    Path = $"/fields/System.History",
                                    Value = commentText
                                }
                            );              
                        }
                        break;
                    case FieldType.Category:                        
                        var categoryValue = field.Values.ToObject<PodioValue[]>();
                        var mappedValue = mapping.CategoryMappings.FirstOrDefault(x => x.PodioValue == categoryValue[0].Value.Text)?.AzureValue;
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

            // Handle files
            foreach (var file in item.Files)
            {
                var podioFile = await podio.FileService.DownloadFile(file);
                using (var stream = new MemoryStream(podioFile.FileContents))
                {
                    var fileReference = await itemClient.CreateAttachmentAsync(stream);

                    patchDocument.Add(
                        new JsonPatchOperation()
                        {
                            Operation = Operation.Add,
                            Path = "/relations/-",
                            Value = new
                            {
                                rel = "AttachedFile",
                                url = fileReference.Url,
                                attributes = new { name = file.Name }
                            }
                        }
                    );
                }
            }

            // Set to current iteration if it's a bug and it has priority 1
            if(patchDocument.Any(x => x.Path == "/fields/Microsoft.VSTS.Common.Priority" && x.Value.ToString() == "1") 
                && string.Equals(item.GetAzureType(app), "bug", StringComparison.CurrentCultureIgnoreCase))
            {
                patchDocument.Add(
                    new JsonPatchOperation()
                    {
                      Operation = Operation.Add,
                      Path = $"/fields/System.IterationPath",
                      Value = "Current"
                    });
            }

            return patchDocument;
        }
    }
}
