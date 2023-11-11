using Microsoft.AspNetCore.Mvc;
using PortalLegisAmbiental.API.Controllers.Base;
using PortalLegisAmbiental.Application.Services.Interfaces;
using PortalLegisAmbiental.Domain.Dtos.Requests;

namespace PortalLegisAmbiental.API.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v{version:apiVersion}/tipos-atos")]
    public class TipoAtoController : BaseController
    {
        private readonly ITipoAtoService _tipoAtoService;

        public TipoAtoController(ITipoAtoService tipoAtoService)
        {
            _tipoAtoService = tipoAtoService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTipoAtoRequest request)
        {
            await _tipoAtoService.Add(request);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(ulong id)
        {
            var response = await _tipoAtoService.GetById(id);
            return Ok(response);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string? name)
        {
            var response = await _tipoAtoService.SearchByName(name);
            return Ok(response);
        }

        [HttpPatch("update/{id}")]
        public async Task<IActionResult> Update(ulong id, TipoAtoRequest request)
        {
            request.Id = id;
            await _tipoAtoService.Update(request);
            return NoContent();
        }

        [HttpPatch("disable")]
        public async Task<IActionResult> Disable(ulong id)
        {
            await _tipoAtoService.Disable(id);
            return NoContent();
        }
    }
}
