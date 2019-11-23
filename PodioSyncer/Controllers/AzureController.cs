using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using PodioAPI;
using PodioAPI.Models;
using PodioAPI.Utils.ItemFields;
using PodioSyncer.Data;
using PodioSyncer.Data.Commands;
using PodioSyncer.Data.Commands.InputModels;
using PodioSyncer.Models.DevOps;
using PodioSyncer.Models.ViewModels;
using PodioSyncer.Options;

namespace PodioSyncer.Controllers
{
    [Route("azure")]
    [ApiController]
    [Authorize]
    public class AzureController : ControllerBase
    {
        private readonly QueryDb _queryDb;
        private readonly IMapper _mapper;
        private readonly ConfigurationOptions _options;

        public AzureController(IOptions<ConfigurationOptions> options, QueryDb queryDb, IMapper mapper)
        {
            _queryDb = queryDb;
            _mapper = mapper;
            _options = options.Value;
        }

        [HttpPost]
        [Route("webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> Webhook(AzureItem item)
        {            
            var link = _queryDb.Links.SingleOrDefault(x => x.AzureId == item.Resource.WorkItemId);
            if (link == null)
            {
                return Ok();
            }
            VssConnection connection = new VssConnection(new Uri(_options.AzureOptions.ProjectUrl), new VssBasicCredential(string.Empty, _options.AzureOptions.AccessToken)); ;
            WorkItemTrackingHttpClient witClient = connection.GetClient<WorkItemTrackingHttpClient>();
            switch (item.EventType)
            {
                case "workitem.updated":
                    if(item.Resource.Fields.Keys.Any(x => x == "System.State"))
                    {
                        var podio = new Podio(_options.PodioOptions.ClientId, _options.PodioOptions.ClientSecret);
                        var app = _queryDb.PodioApps.SingleOrDefault(x => x.Id == link.PodioAppId);
                        await podio.AuthenticateWithApp(app.PodioAppId, app.AppToken);
                        var mapping = _queryDb.FieldMappings.Include(x => x.CategoryMappings).FirstOrDefault(x => x.AzureFieldName == "System.State");
                        var azureValue = ((JsonElement)item.Resource.Fields.GetOrAddValue("System.State")).GetProperty("newValue").GetString();
                        var catMapping = mapping.CategoryMappings.FirstOrDefault(x => x.AzureValue == azureValue);
                        var podioItem = new Item();
                        podioItem.ItemId = link.PodioId;
                        var catField = podioItem.Field<CategoryItemField>(mapping.PodioFieldName);
                        catField.OptionId = catMapping.PodioValueId;
                        await podio.ItemService.UpdateItem(podioItem, hook: false);
                    }
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