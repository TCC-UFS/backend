using Microsoft.AspNetCore.Mvc;
using PortalLegisAmbiental.API.Controllers.Base;
using PortalLegisAmbiental.Domain.IRepositories;

namespace PortalLegisAmbiental.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/healthcheck")]
    public class HealthcheckController : BaseController
    {
        private readonly ISearchRepository _searchRepository;
        public HealthcheckController(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("I'm alive and getting.");
        }

        [HttpPost]
        public IActionResult Post()
        {
            return Ok("I'm alive and postting.");
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            return Ok("I'm alive and deleting.");
        }
    }
}