using Microsoft.AspNetCore.Mvc;
using PortalLegisAmbiental.API.Controllers.Base;
using PortalLegisAmbiental.Application.Services.Interfaces;
using PortalLegisAmbiental.Domain.Dtos.Requests;

namespace PortalLegisAmbiental.API.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v{version:apiVersion}/jurisdicoes")]
    public class JurisdicaoController : BaseController
    {
        private readonly IJurisdicaoService _jurisdicaoService;

        public JurisdicaoController(IJurisdicaoService jurisdicaoService)
        {
            _jurisdicaoService = jurisdicaoService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddJurisdicaoRequest request)
        {
            await _jurisdicaoService.Add(request);
            return NoContent();
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _jurisdicaoService.GetAll();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(ulong id)
        {
            var response = await _jurisdicaoService.GetById(id);
            return Ok(response);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string? state)
        {
            var response = await _jurisdicaoService.SearchByState(state);
            return Ok(response);
        }

        [HttpGet("serach/{sigla}")]
        public async Task<IActionResult> SearchState(string sigla)
        {
            var response = await _jurisdicaoService.GetBySigla(sigla);
            return Ok(response);
        }

        [HttpGet("{ambito}")]
        public async Task<IActionResult> SearchAmbito(string ambito)
        {
            var response = await _jurisdicaoService.SearchByAmbito(ambito);
            return Ok(response);
        }

        [HttpPatch("update/{id}")]
        public async Task<IActionResult> Update(ulong id, UpdateJurisdicaoRequest request)
        {
            request.Id = id;
            await _jurisdicaoService.Update(request);
            return NoContent();
        }

        [HttpPatch("disable")]
        public async Task<IActionResult> Disable(ulong id)
        {
            await _jurisdicaoService.Disable(id);
            return NoContent();
        }
    }
}
