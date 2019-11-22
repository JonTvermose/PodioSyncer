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
using PodioSyncer.Models.ViewModels;
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

        public async Task<string> SyncPodioItemToAzure(CreateLink createLinkCommand, Podio podio, PodioApp app, Item item)
        {            
            var link = _queryDb.Links.SingleOrDefault(x => x.PodioId == item.ItemId);

            VssConnection connection = null;
            WorkItemTrackingHttpClient witClient = null;
            var type = item.GetAzureType(app);
            if (type == null)
            {
                throw new Exception($"Podio type not mapped.");
            }
            connection = new VssConnection(new Uri(_options.AzureOptions.ProjectUrl), new VssBasicCredential(string.Empty, _options.AzureOptions.AccessToken));
            witClient = connection.GetClient<WorkItemTrackingHttpClient>();
            var azureUrl = await CreateAzureItem(podio, item, witClient, createLinkCommand, app);            
            return azureUrl;
        }

        public async Task HandlePodioHook(int appId, PodioWebhook hook, VerifyWebhookCommand verifyCommand, CreateLink createLinkCommand, UpdateLink updateLinkCommand)
        {
            var podio = new Podio(_options.PodioOptions.ClientId, _options.PodioOptions.ClientSecret);
            var app = _queryDb.PodioApps.SingleOrDefault(x => x.PodioAppId == appId);
            await podio.AuthenticateWithApp(appId, app.AppToken);

            //var item = await podio.ItemService.GetItemByAppItemId(appId, int.Parse(hook.item_id));

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
                    type = item.GetAzureType(app);
                    if (type == null)
                    {
                        // No need to create a devops item.
                        return;
                    }
                    connection = new VssConnection(new Uri(_options.AzureOptions.ProjectUrl), new VssBasicCredential(string.Empty, _options.AzureOptions.AccessToken));
                    witClient = connection.GetClient<WorkItemTrackingHttpClient>();
                    await CreateAzureItem(podio, item, witClient, createLinkCommand, app);
                    break;
                case "item.update":
                    var revision = item.CurrentRevision.Revision;
                    if (link.PodioRevision >= revision)
                    {
                        // Allready processed this revision
                        return;
                    }
                    type = item.GetAzureType(app);
                    connection = new VssConnection(new Uri(_options.AzureOptions.ProjectUrl), new VssBasicCredential(string.Empty, _options.AzureOptions.AccessToken));
                    witClient = connection.GetClient<WorkItemTrackingHttpClient>();
                    if (link == null && type != null)
                    {
                        // Will happen for items allready created before implementing this system or if their type is set to a type we should handle
                        await CreateAzureItem(podio, item, witClient, createLinkCommand, app);
                        return;
                    }
                    var changes = await item.GetChangesAsync(_queryDb, witClient, podio, app);
                    var syncChanges = await podio.ItemService.GetItemRevisionDifference(int.Parse(hook.item_id), link.PodioRevision, revision);                    
                    foreach(var change in syncChanges)
                    {
                        var map = change.ExternalId; // Get mapping
                        var changeValue = change.To.ToObject<PodioValue>().Value.Text;
                    }
                    var wItem = await witClient.UpdateWorkItemAsync(changes, link.AzureId);
                    var updateRevision = await UpdateAzureComments(item, witClient, link);
                    link.AzureRevision = updateRevision; 
                    link.PodioRevision = revision;
                    updateLinkCommand.InputModel = link;
                    updateLinkCommand.Run();
                    break;
                case "item.delete":
                    break;
            }
        }

        private async Task<string> CreateAzureItem(Podio podio, Item item, WorkItemTrackingHttpClient witClient, CreateLink createLinkCommand, PodioApp app)
        {
            var patchOperations = await item.GetChangesAsync(_queryDb, witClient, podio, app);
            var createResult = await witClient.CreateWorkItemAsync(patchOperations, _options.AzureOptions.ProjectGuid, item.GetAzureType(app), suppressNotifications: true);

            var link = new PodioAzureItemLink
            {
                AzureId = createResult.Id.Value,
                PodioId = item.ItemId,
                PodioRevision = item.CurrentRevision.Revision,
                AzureRevision = createResult.Rev.Value,
                PodioAppId = app.Id
            };

            link.AzureRevision = await UpdateAzureComments(item, witClient, link); ;

            createLinkCommand.InputModel = link;
            createLinkCommand.Run();

            // TODO Sync to Podio
            // Create Comment with link to azure item
            var url = $"{_options.AzureOptions.ProjectUrl}/{_options.AzureOptions.ProjectGuid}/_workitems/edit/{createResult.Url.Split('/').Last()}";
            return url;
        }

        private async Task<int> UpdateAzureComments(Item item, WorkItemTrackingHttpClient witClient, PodioAzureItemLink link)
        {
            var newRevision = link.AzureRevision;
            var azureComments = await witClient.GetCommentsAsync(link.AzureId);
            foreach (var comment in item.Comments)
            {
                var header = $"{comment.CommentId} | Comment by: {comment.CreatedBy.Name}</br>";
                if (azureComments.Comments.Any(x => x.Text.StartsWith(header)))
                {
                    continue;
                }
                var result = new JsonPatchDocument();
                header += comment.Value;
                result.Add(new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = $"/fields/System.History",
                    Value = header
                });
                // Can only update one comment at the time
                var updateResult = await witClient.UpdateWorkItemAsync(result, link.AzureId, suppressNotifications: true);
                newRevision = updateResult.Rev.Value;
            }
            return newRevision;
        }
    }
}
