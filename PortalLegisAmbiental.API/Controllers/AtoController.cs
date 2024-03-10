using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalLegisAmbiental.API.Controllers.Base;
using PortalLegisAmbiental.API.Filters;
using PortalLegisAmbiental.Application.Services.Interfaces;
using PortalLegisAmbiental.Domain.Dtos.Requests;

namespace PortalLegisAmbiental.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/atos")]
    public class AtoController : BaseController
    {
        private readonly IAtoService _atoService;
        private readonly IAccessService _accessService;

        public AtoController(IAtoService atoService, IAccessService accessService)
        {
            _atoService = atoService;
            _accessService = accessService;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = await _atoService.GetStats();
            return Ok(stats);
        }

        [Authorize]
        [Permission]
        [HttpPost("files")]
        public async Task<IActionResult> AddWithFile([FromForm] AddAtoRequest request)
        {
            var user = _accessService.GetLoggedUser(HttpContext.User);

            if (user == null)
                return Failed("User not found.");

            request.CreatedById = user.Id;
            request.CaminhoArquivo = string.Empty;
            var ato = await _atoService.Add(request);
            return Ok(ato);
        }

        [Authorize]
        [Permission]
        [HttpPost]
        public async Task<IActionResult> Add(AddAtoRequest request)
        {
            var user = _accessService.GetLoggedUser(HttpContext.User);

            if (user == null)
                return Failed("User not found.");

            request.CreatedById = user.Id;
            request.CaminhoArquivo = string.Empty;
            var ato = await _atoService.Add(request);
            return Ok(ato);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string? numero, string? tipo, string? jurisdicao, string? order, string? ano)
        {
            if (string.IsNullOrEmpty(order)) order = "desc";
            var response = await _atoService.Search(numero, tipo, jurisdicao, ano, order);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(ulong id)
        {
            var response = await _atoService.GetById(id);
            return Ok(response);
        }

        [Authorize]
        [Permission]
        [HttpPatch("update/{id}")]
        public async Task<IActionResult> Update(ulong id, [FromForm] UpdateAtoRequest request)
        {
            request.Id = id;
            
            if (request.File != null)
                request.CaminhoArquivo = Path.GetFullPath("../wwwroot");

            await _atoService.Update(request);
            return NoContent();
        }

        [Authorize]
        [Permission]
        [HttpDelete("disable")]
        public async Task<IActionResult> Disable(ulong id)
        {
            await _atoService.Disable(id);
            return NoContent();
        }
    }
}
