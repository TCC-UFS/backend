using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalLegisAmbiental.API.Controllers.Base;
using PortalLegisAmbiental.API.Filters;

namespace PortalLegisAmbiental.API.Controllers
{
    [Authorize]
    [Permission]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/healthcheck")]
    public class HealthcheckController : BaseController
    {
        public HealthcheckController() { }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("I'm alive and geting.");
        }

        [HttpPost]
        public IActionResult Post()
        {
            return Ok("I'm alive and posting.");
        }

        [HttpPatch]
        public IActionResult Patch()
        {
            return Ok("I'm alive and patching.");
        }

        [HttpPut]
        public IActionResult Put()
        {
            return Ok("I'm alive and puting");
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            return Ok("I'm alive and deleting.");
        }
    }
}