using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PodioSyncer.Data;
using PodioSyncer.Data.Commands;
using PodioSyncer.Data.Commands.InputModels;
using PodioSyncer.Models.Podio;
using PodioSyncer.Models.ViewModels;
using PodioSyncer.Options;
using PodioSyncer.Services;

namespace PodioSyncer.Controllers
{
    [Route("podio")]
    [ApiController]
    public class PodioController : ControllerBase
    {
        private readonly ConfigurationOptions _options;
        private readonly QueryDb _queryDb;
        private readonly IMapper _mapper;
        private readonly SyncService _syncService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PodioController(IOptionsMonitor<ConfigurationOptions> options, QueryDb queryDb, IMapper mapper, SyncService syncService, IHttpContextAccessor httpContextAccessor)
        {
            _options = options.CurrentValue;
            _queryDb = queryDb;
            _mapper = mapper;
            _syncService = syncService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Route("webhook/{appId}")]
        public async Task<IActionResult> Webhook(int appId, PodioWebhook hook, [FromServices] VerifyWebhookCommand verifyCommand, [FromServices] CreateLink createLinkCommand, [FromServices] UpdateLink updateLinkCommand)
        {
            await _syncService.HandlePodioHook(appId, hook, verifyCommand, createLinkCommand, updateLinkCommand);
            return Ok();
        }

        [HttpGet]
        [Route("getpodioapps")]
        public IActionResult GetPodioApps()
        {
            var podioApps = _queryDb.PodioApps.ProjectTo<PodioAppViewModel>(_mapper.ConfigurationProvider).ToList(); 
            foreach(var app in podioApps)
            {
                app.WebhookUrl = $"{_httpContextAccessor.HttpContext.Request.Host.Value}/podio/webhook/{app.PodioAppId}";
            }
            return Ok(podioApps);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken(Order = 1001)]
        [Route("syncpodioitem")]
        public async Task<IActionResult> SyncPodioItem([FromBody] PodioSyncItemViewModel inputModel, [FromServices] CreateLink createLinkCommand)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // TODO check that item has not been synced
            var azureLink = await _syncService.SyncPodioItemToAzure(inputModel, createLinkCommand);

            return Ok(azureLink);
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
        [IgnoreAntiforgeryToken(Order = 1001)]
        [Route("deletepodioapp/{id}")]
        public IActionResult DeletePodioApp([FromRoute] int id, [FromServices] DeletePodioApp command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            command.InputModel = id;
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