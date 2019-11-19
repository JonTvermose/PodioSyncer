using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using PodioAPI;
using PodioAPI.Models;
using PodioSyncer.Data;
using PodioSyncer.Data.Commands;
using PodioSyncer.Data.Models;
using PodioSyncer.Extensions;
using PodioSyncer.Models.Podio;
using PodioSyncer.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace PodioSyncer.Services
{
    public class SyncService
    {
        private ConfigurationOptions _options;
        private QueryDb _queryDb;
        private IMapper _mapper;

        public SyncService(IOptionsMonitor<ConfigurationOptions> options, QueryDb queryDb, IMapper mapper)
        {
            _options = options.CurrentValue;
            _queryDb = queryDb;
            _mapper = mapper;
        }

        public async Task HandlePodioHook(int appId, PodioWebhook hook, VerifyWebhookCommand verifyCommand, CreateLink createLinkCommand, UpdateLink updateLinkCommand)
        {
            var podio = new Podio(_options.PodioOptions.ClientId, _options.PodioOptions.ClientSecret);
            var app = _queryDb.PodioApps.SingleOrDefault(x => x.PodioAppId == appId.ToString());
            await podio.AuthenticateWithApp(appId, app.AppToken);

            var item = await podio.ItemService.GetItem(int.Parse(hook.item_id));
            var link = _queryDb.Links.SingleOrDefault(x => x.PodioId == item.ItemId);

            VssConnection connection = null;
            WorkItemTrackingHttpClient witClient = null;

            string type = null;
            switch (hook.type)
            {
                case "hook.verify":
                    await podio.HookService.ValidateHookVerification(int.Parse(hook.hook_id), hook.code);
                    verifyCommand.PodioAppId = appId;
                    verifyCommand.Run();
                    break;
                case "item.create":
                    type = item.GetAzureType();
                    if (type == null)
                    {
                        // No need to create a devops item.
                        return;
                    }
                    connection = new VssConnection(new Uri(_options.AzureOptions.ProjectUrl), new VssBasicCredential(string.Empty, _options.AzureOptions.AccessToken));
                    witClient = connection.GetClient<WorkItemTrackingHttpClient>();
                    link = await CreateAzureItem(app.Id, podio, item, witClient, createLinkCommand);
                    break;
                case "item.update":
                    var revision = item.CurrentRevision.Revision;
                    if (link.PodioRevision >= revision)
                    {
                        // Allready processed this revision
                        return;
                    }
                    type = item.GetAzureType();
                    connection = new VssConnection(new Uri(_options.AzureOptions.ProjectUrl), new VssBasicCredential(string.Empty, _options.AzureOptions.AccessToken));
                    witClient = connection.GetClient<WorkItemTrackingHttpClient>();
                    if (link == null && type != null)
                    {
                        // Will happen for items allready created before implementing this system or if their type is set to a type we should handle
                        await CreateAzureItem(app.Id, podio, item, witClient, createLinkCommand);
                        return;
                    }
                    var changes = await item.GetChangesAsync(_queryDb, witClient, podio);
                    var commentUpdates = await UpdateAzureComments(item, witClient, link);
                    changes.AddRange(commentUpdates.AsEnumerable());
                    var wItem = await witClient.UpdateWorkItemAsync(changes, link.AzureId);
                    // update link revisions to avoid infinite loops
                    link.PodioRevision = revision;
                    link.AzureRevision = wItem.Rev.Value; // TODO should this be +1 ?
                    updateLinkCommand.InputModel = link;
                    updateLinkCommand.Run();
                    break;
                case "item.delete":
                    break;
            }
        }

        private async Task<PodioAzureItemLink> CreateAzureItem(int appId, Podio podio, Item item, WorkItemTrackingHttpClient witClient, CreateLink createLinkCommand)
        {
            var patchOperations = await item.GetChangesAsync(_queryDb, witClient, podio);
            var createResult = await witClient.CreateWorkItemAsync(patchOperations, _options.AzureOptions.ProjectGuid, item.GetAzureType());

            var link = new PodioAzureItemLink
            {
                AzureId = createResult.Id.Value,
                PodioId = item.ItemId,
                PodioRevision = item.CurrentRevision.Revision,
                AzureRevision = createResult.Rev.Value,
                PodioAppId = appId
            };

            var commentOperations = await UpdateAzureComments(item, witClient, link);
            if (commentOperations.Any())
            {
                var updatedItem = await witClient.UpdateWorkItemAsync(commentOperations, link.AzureId);
                link.AzureRevision = updatedItem.Rev.Value;
            }

            createLinkCommand.InputModel = link;
            createLinkCommand.Run();
            return link;
        }

        private async Task<JsonPatchDocument> UpdateAzureComments(Item item, WorkItemTrackingHttpClient witClient, PodioAzureItemLink link)
        {
            var result = new JsonPatchDocument();
            var azureComments = await witClient.GetCommentsAsync(link.AzureId);
            foreach (var comment in item.Comments)
            {
                var header = $"{comment.CommentId} | Comment by: {comment.CreatedBy.Name}</br>";
                if (azureComments.Comments.Any(x => x.Text.StartsWith(header)))
                {
                    continue;
                }
                header += comment.Value; // TODO might be RichValue ? 
                result.Add(new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = $"/fields/System.History",
                    Value = header
                });
            }
            return result;
        }
    }
}
