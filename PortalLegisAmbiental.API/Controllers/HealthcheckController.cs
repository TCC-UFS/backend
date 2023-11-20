using Microsoft.AspNetCore.Authorization;
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
            return Ok("I'm alive and geting.");
        }

        [HttpPost]
        [Authorize]
        public async IActionResult Post()
        {
            var result = await _searchRepository.Teste();
            return Ok(result);
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