using Microsoft.AspNetCore.Mvc;
using PortalLegisAmbiental.API.Controllers.Base;
using PortalLegisAmbiental.Application.Services.Interfaces;
using PortalLegisAmbiental.Domain.Dtos.Requests;

namespace PortalLegisAmbiental.API.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v{version:apiVersion}/atos")]
    public class AtoController : BaseController
    {
        private readonly IAtoService _atoService;

        public AtoController(IAtoService atoService)
        {
            _atoService = atoService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddAtoRequest request)
        {
            var ato = await _atoService.Add(request);
            return Ok(ato);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string? numero, string? tipo, string? jurisdicao, string? order)
        {
            if (string.IsNullOrEmpty(order)) order = "desc";
            var response = await _atoService.Search(numero, tipo, jurisdicao, order);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(ulong id)
        {
            var response = await _atoService.GetById(id);
            return Ok(response);
        }

        [HttpPatch("update/{id}")]
        public async Task<IActionResult> Update(ulong id, UpdateAtoRequest request)
        {
            request.Id = id;
            await _atoService.Update(request);
            return NoContent();
        }

        [HttpPatch("disable")]
        public async Task<IActionResult> Disable(ulong id)
        {
            await _atoService.Disable(id);
            return NoContent();
        }
    }
}
