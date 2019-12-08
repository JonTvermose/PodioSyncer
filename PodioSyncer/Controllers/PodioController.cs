using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PodioAPI;
using PodioSyncer.Data;
using PodioSyncer.Data.Commands;
using PodioSyncer.Data.Commands.InputModels;
using PodioSyncer.Middleware;
using PodioSyncer.Models.Podio;
using PodioSyncer.Models.ViewModels;
using PodioSyncer.Options;
using PodioSyncer.Services;

namespace PodioSyncer.Controllers
{
    [Route("podio")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(DblExceptionFilter))]
    public class PodioController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly SyncService _syncService;

        public PodioController(IMapper mapper, SyncService syncService)
        {
            _mapper = mapper;
            _syncService = syncService;
        }

        [HttpPost]
        [Route("webhook/{appId}")]
        [AllowAnonymous]
        public async Task<IActionResult> Webhook(int appId, [FromForm] PodioWebhook hook, 
          [FromServices] VerifyWebhookCommand verifyCommand, 
          [FromServices] CreateLink createLinkCommand, 
          [FromServices] UpdateLink updateLinkCommand)
        {
            await _syncService.HandlePodioHook(appId, hook, verifyCommand, createLinkCommand, updateLinkCommand);
            return Ok();
        }

    }
}