using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PodioAPI;
using PodioSyncer.Data;
using PodioSyncer.Data.Commands;
using PodioSyncer.Data.Models;
using PodioSyncer.Models.Podio;
using PodioSyncer.Options;

namespace PodioSyncer.Controllers
{
    [Route("api/podio")]
    [ApiController]
    public class PodioController : ControllerBase
    {
        private readonly PodioOptions _options;
        private readonly QueryDb _queryDb;

        public PodioController(ConfigurationOptions options, QueryDb queryDb)
        {
            _options = options.PodioOptions;
            _queryDb = queryDb;
        }

        [HttpPost]
        [Route("webhook/{appId}")]
        public async Task<IActionResult> Webhook(int appId, PodioWebhook hook, [FromServices] VerifyWebhookCommand verifyCommand)
        {
            var podio = new Podio(_options.ClientId, _options.ClientSecret);
            var app = _queryDb.PodioApps.SingleOrDefault(x => x.PodioAppId == appId); 
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
        public async Task<IActionResult> GetPodioApps()
        {
            var podioApps = new List<PodioApp>(); // _queryDb.PodioApps.ToList(); // TODO
            podioApps.Add(new PodioApp
            {
                Name = "Test1",
                PodioAppId = 354636,
                Verified = false,
                Active = true,
                Id = 1,
                WebhookUrl = Url.Action("Webhook", "Podio")
            });
            return Ok(podioApps);
        }

    }
}