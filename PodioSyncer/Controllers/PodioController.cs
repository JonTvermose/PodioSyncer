using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PodioAPI;
using PodioSyncer.Data;
using PodioSyncer.Data.Commands;
using PodioSyncer.Data.Commands.InputModels;
using PodioSyncer.Models.Podio;
using PodioSyncer.Models.ViewModels;
using PodioSyncer.Options;

namespace PodioSyncer.Controllers
{
    [Route("api/podio")]
    [ApiController]
    public class PodioController : ControllerBase
    {
        private readonly PodioOptions _options;
        private readonly QueryDb _queryDb;
        private readonly IMapper _mapper;

        public PodioController(ConfigurationOptions options, QueryDb queryDb, IMapper mapper)
        {
            _options = options.PodioOptions;
            _queryDb = queryDb;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("webhook/{appId}")]
        public async Task<IActionResult> Webhook(int appId, PodioWebhook hook, [FromServices] VerifyWebhookCommand verifyCommand)
        {
            var podio = new Podio(_options.ClientId, _options.ClientSecret);
            var app = _queryDb.PodioApps.SingleOrDefault(x => x.PodioAppId == appId.ToString()); 
            await podio.AuthenticateWithApp(appId, app.AppToken);

            switch (hook.type)
            {
                case "hook.verify":
                    await podio.HookService.ValidateHookVerification(int.Parse(hook.hook_id), hook.code);
                    verifyCommand.PodioAppId = appId;
                    verifyCommand.Run();
                    break;
                case "item.create":
                    var createdItem = await podio.ItemService.GetItem(int.Parse(hook.item_id));
                    break;
                case "item.update":
                    var updatedItem = await podio.ItemService.GetItem(int.Parse(hook.item_id));
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