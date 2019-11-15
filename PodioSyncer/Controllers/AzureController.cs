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
using PodioSyncer.Models.DevOps;
using PodioSyncer.Models.ViewModels;
using PodioSyncer.Options;

namespace PodioSyncer.Controllers
{
    [Route("api/azure")]
    [ApiController]
    public class AzureController : ControllerBase
    {
        private readonly PodioOptions _options;
        private readonly QueryDb _queryDb;
        private readonly IMapper _mapper;

        public AzureController(ConfigurationOptions options, QueryDb queryDb, IMapper mapper)
        {
            _options = options.PodioOptions;
            _queryDb = queryDb;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("webhook")]
        public async Task<IActionResult> Webhook(AzureItem item)
        {            
            switch (item.EventType)
            {
                case "workitem.updated":
                    break;
                case "item.create":
                    break;
                case "item.update":
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