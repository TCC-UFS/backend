using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalLegisAmbiental.API.Controllers.Base;
using PortalLegisAmbiental.API.Filters;
using PortalLegisAmbiental.Application.Services.Interfaces;
using PortalLegisAmbiental.Domain.Dtos.Requests;

namespace PortalLegisAmbiental.API.Controllers
{
    [Authorize]
    [Permission]
    [ApiVersion("1")]
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _tipoAtoService.GetAll();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(ulong id)
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
        public async Task<IActionResult> Update(ulong id, UpdateTipoAtoRequest request)
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
