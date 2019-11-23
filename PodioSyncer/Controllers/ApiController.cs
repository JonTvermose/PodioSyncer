using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PodioSyncer.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class ApiController: ControllerBase
    {
    }
}
