using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using PodioAPI;
using PodioSyncer.Data;
using PodioSyncer.Data.Commands;
using PodioSyncer.Data.Commands.InputModels;
using PodioSyncer.Data.Models;
using PodioSyncer.Models.Podio;
using PodioSyncer.Models.ViewModels;
using PodioSyncer.Options;

namespace PodioSyncer.Controllers
{
    [Route("api/podio")]
    [ApiController]
    public class PodioController : ControllerBase
    {
        private readonly ConfigurationOptions _options;
        private readonly QueryDb _queryDb;
        private readonly IMapper _mapper;

        public PodioController(ConfigurationOptions options, QueryDb queryDb, IMapper mapper)
        {
            _options = options;
            _queryDb = queryDb;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("webhook/{appId}")]
        public async Task<IActionResult> Webhook(int appId, PodioWebhook hook, [FromServices] VerifyWebhookCommand verifyCommand, [FromServices] CreateLink createLinkCommand, [FromServices] UpdateLink updateLinkCommand)
        {
            var podio = new Podio(_options.PodioOptions.ClientId, _options.PodioOptions.ClientSecret);
            var app = _queryDb.PodioApps.SingleOrDefault(x => x.PodioAppId == appId.ToString()); 
            await podio.AuthenticateWithApp(appId, app.AppToken);

            var item = await podio.ItemService.GetItem(int.Parse(hook.item_id));
            var link = _queryDb.Links.SingleOrDefault(x => x.PodioId == item.ItemId);

            VssConnection connection = new VssConnection(new Uri(_options.AzureOptions.ProjectUrl), new VssBasicCredential(string.Empty, _options.AzureOptions.AccessToken));
            var witClient = connection.GetClient<WorkItemTrackingHttpClient>();

            switch (hook.type)
            {
                case "hook.verify":
                    await podio.HookService.ValidateHookVerification(int.Parse(hook.hook_id), hook.code);
                    verifyCommand.PodioAppId = appId;
                    verifyCommand.Run();
                    break;
                case "item.create":
                    if(link != null)
                    {
                        // TODO Shouldnt happen
                    }
                   
                    var createResult = await witClient.CreateWorkItemAsync(null, null, ""); // TODO

                    link = new PodioAzureItemLink
                    {
                        AzureId = createResult.Id.Value,
                        AzureRevision = createResult.Rev.Value,
                        PodioId = item.ItemId,
                        PodioRevision = item.CurrentRevision.Revision
                    };
                    createLinkCommand.InputModel = link;
                    createLinkCommand.Run();

                    break;
                case "item.update":
                    var revision = item.CurrentRevision.Revision;
                    if(link == null)
                    {
                        // TODO Shouldnt happpen
                        // Will happen for items allready created before implementing this system
                    } else if (link.PodioRevision >= revision)
                    {
                        // Allready processed this revision
                        return Ok();
                    }
                    var wItem = await witClient.UpdateWorkItemAsync(null, link.AzureId);

                    // TODO update item
                    // update link revisions to avoid infinite loops
                    link.PodioRevision = revision;
                    link.AzureRevision = wItem.Rev.Value;
                    updateLinkCommand.InputModel = link;
                    updateLinkCommand.Run();
                    break;
                case "item.delete":
                    break;
            }
            return Ok();
        }

        [HttpGet]
        [Route("getpodioapps")]
        public IActionResult GetPodioApps()
        {
            var podioApps = _queryDb.PodioApps.ProjectTo<PodioAppViewModel>(_mapper.ConfigurationProvider).ToList(); 
            return Ok(podioApps);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken(Order = 1001)]
        [Route("createpodioapp")]
        public IActionResult CreatePodioApp([FromBody] PodioAppInputModel inputModel, [FromServices] CreatePodioApp command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            command.InputModel = inputModel;
            command.Run();

            return Ok();
        }

        [HttpPost]
        [Route("editpodioapp")]
        public IActionResult EditPodioApp(PodioAppInputModel inputModel, [FromServices] UpdatePodioApp command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            command.InputModel = inputModel;
            command.Run();

            return Ok();
        }

    }
}