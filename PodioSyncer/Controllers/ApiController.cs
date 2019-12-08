using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using PodioAPI;
using PodioSyncer.Data;
using PodioSyncer.Data.Commands;
using PodioSyncer.Data.Commands.InputModels;
using PodioSyncer.Data.Models;
using PodioSyncer.Middleware;
using PodioSyncer.Models.ViewModels;
using PodioSyncer.Options;
using PodioSyncer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PodioSyncer.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(DblExceptionFilter))]
    public class ApiController: ControllerBase
    {
        private readonly QueryDb _queryDb;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SyncService _syncService;
        private readonly ConfigurationOptions _options;

        public ApiController(QueryDb queryDb, IMapper mapper, IHttpContextAccessor httpContextAccessor, SyncService syncService, IOptions<ConfigurationOptions> options)
        {
            _queryDb = queryDb;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _syncService = syncService;
            _options = options.Value;
        }

        [HttpGet]
        [Route("getlinks")]
        public async Task<IActionResult> GetLinks(int id)
        {
            var result = await _queryDb.PodioApps
                .Include(x => x.PodioAzureItemLinks)
                .ThenInclude(x => x.SyncEvents)
                .Where(x => x.PodioAppId == id)
                .SelectMany(x => x.PodioAzureItemLinks)
                .Select(x => new
                {
                    x.AzureUrl,
                    x.PodioUrl,
                    SyncedDate = x.SyncEvents.OrderBy(x => x.SyncDate).First().SyncDate.ToString("HH:mm dd-MM-yyyy")                    
                })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("getsyncevents")]
        public async Task<IActionResult> GetSyncEvents()
        {
            var result = await _queryDb.SyncEvents                
                .Include(x => x.PodioAzureItemLink)
                .OrderByDescending(x => x.SyncDate)
                .Select(x => new
                {
                    x.PodioAzureItemLink.AzureUrl,
                    x.PodioAzureItemLink.PodioUrl,
                    SyncedDate = x.SyncDate.ToString("HH:mm dd-MM-yyyy"),
                    initiator = x.Initiator.ToString()
                })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("getpodioapps")]
        public IActionResult GetPodioApps()
        {
            var podioApps = _queryDb.PodioApps.ProjectTo<PodioAppViewModel>(_mapper.ConfigurationProvider).ToList();
            foreach (var app in podioApps)
            {
                app.WebhookUrl = $"{_httpContextAccessor.HttpContext.Request.Host.Value}/podio/webhook/{app.PodioAppId}";
            }
            return Ok(podioApps);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken(Order = 1001)]
        [Route("syncpodioitem")]
        public async Task<IActionResult> SyncPodioItem([FromBody] PodioSyncItemViewModel model, [FromServices] CreateLink createLinkCommand)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var podio = new Podio(_options.PodioOptions.ClientId, _options.PodioOptions.ClientSecret);
            var app = _queryDb.PodioApps.SingleOrDefault(x => x.PodioAppId == model.PodioAppId);
            await podio.AuthenticateWithApp(model.PodioAppId, app.AppToken);
            var item = await podio.ItemService.GetItemByAppItemId(model.PodioAppId, model.AppItemId);

            if (_queryDb.Links.Any(x => x.PodioId == item.ItemId))
            {
                return Ok(new { ok = false });
            }
            var azureLink = await _syncService.SyncPodioItemToAzure(createLinkCommand, podio, app, item);

            return Ok(new { url = azureLink, ok = true });
        }

    [HttpPost]
    [IgnoreAntiforgeryToken(Order = 1001)]
    [Route("createlink")]
    public async Task<IActionResult> CreateLink([FromBody] PodioSyncItemViewModel model, 
      [FromServices] CreateLink createLinkCommand, 
      [FromServices] CreateSyncEvent createSyncEventCommand)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var podio = new Podio(_options.PodioOptions.ClientId, _options.PodioOptions.ClientSecret);
      var app = _queryDb.PodioApps.SingleOrDefault(x => x.PodioAppId == model.PodioAppId);
      await podio.AuthenticateWithApp(model.PodioAppId, app.AppToken);
      var podioItem = await podio.ItemService.GetItemByAppItemId(model.PodioAppId, model.AppItemId);

      if (_queryDb.Links.Any(x => x.PodioId == podioItem.ItemId))
      {
        return Ok(new { ok = false });
      }
      VssConnection connection = new VssConnection(new Uri(_options.AzureOptions.ProjectUrl), new VssBasicCredential(string.Empty, _options.AzureOptions.AccessToken)); ;
      WorkItemTrackingHttpClient witClient = connection.GetClient<WorkItemTrackingHttpClient>();
      var azureItem = await witClient.GetWorkItemAsync(model.AzureItemId);
      
      var link = new PodioAzureItemLink
      {
        AzureId = azureItem.Id.Value,
        AzureUrl = azureItem.Url,
        AzureRevision = azureItem.Rev.Value,
        PodioAppId = app.Id,
        PodioUrl = podioItem.Link,
        PodioRevision = podioItem.CurrentRevision.Revision,
        PodioId = podioItem.ItemId
      };
      createLinkCommand.InputModel = link;
      createLinkCommand.Run();

      createSyncEventCommand.InputModel = new SyncEvent
      {
        AzureRevision = link.AzureRevision,
        Initiator = Initiator.Manuel,
        PodioAzureItemLinkId = link.Id,
        PodioRevision = link.PodioRevision,
        SyncDate = DateTime.UtcNow
      };
      createSyncEventCommand.Run();
      return Ok(new { ok = true });
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
        [IgnoreAntiforgeryToken(Order = 1001)]
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
