using Microsoft.AspNetCore.Mvc;
using PortalLegisAmbiental.API.Controllers.Base;

namespace PortalLegisAmbiental.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/healthcheck")]
    public class HealthcheckController : BaseController
    {
        public HealthcheckController() { }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("I'm alive and runing.");
        }
    }
}