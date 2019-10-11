using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PodioAPI;
using PodioSyncer.Data;
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
        public async Task<IActionResult> Webhook(int appId, PodioWebhook hook)
        {
            var podio = new Podio(_options.ClientId, _options.ClientSecret);

            var appToken = _queryDb.PodioApps.SingleOrDefault(x => x.PodioAppId == appId)?.AppToken; 
            await podio.AuthenticateWithApp(appId, appToken);

            switch (hook.type)
            {
                case "hook.verify":
                    await podio.HookService.ValidateHookVerification(int.Parse(hook.hook_id), hook.code);
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

    }
}