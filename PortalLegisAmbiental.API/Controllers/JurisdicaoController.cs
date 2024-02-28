using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalLegisAmbiental.API.Controllers.Base;
using PortalLegisAmbiental.API.Filters;
using PortalLegisAmbiental.Application.Services.Interfaces;
using PortalLegisAmbiental.Domain.Dtos.Requests;

namespace PortalLegisAmbiental.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/jurisdicoes")]
    public class JurisdicaoController : BaseController
    {
        private readonly IJurisdicaoService _jurisdicaoService;

        public JurisdicaoController(IJurisdicaoService jurisdicaoService)
        {
            _jurisdicaoService = jurisdicaoService;
        }

        [Authorize]
        [Permission]
        [HttpPost]
        public async Task<IActionResult> Add(AddJurisdicaoRequest request)
        {
            await _jurisdicaoService.Add(request);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string? state, string? sigla, string? ambito, string? order)
        {
            var response = await _jurisdicaoService.Search(state, sigla, ambito, order ?? "asc");
            return Ok(response);
        }

        [Authorize]
        [Permission]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(ulong id)
        {
            var response = await _jurisdicaoService.GetById(id);
            return Ok(response);
        }

        [Authorize]
        [Permission]
        [HttpPatch("update/{id}")]
        public async Task<IActionResult> Update(ulong id, UpdateJurisdicaoRequest request)
        {
            request.Id = id;
            await _jurisdicaoService.Update(request);
            return NoContent();
        }

        [Authorize]
        [Permission]
        [HttpDelete("disable")]
        public async Task<IActionResult> Disable(ulong id)
        {
            await _jurisdicaoService.Disable(id);
            return NoContent();
        }
    }
}
