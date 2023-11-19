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
    [Route("api/v{version:apiVersion}/permissoes")]
    public class PermissaoController : BaseController
    {
        private readonly IPermissaoService _permissaoService;

        public PermissaoController(IPermissaoService permissaoService)
        {
            _permissaoService = permissaoService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddPermissaoRequest request)
        {
            await _permissaoService.Add(request);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string? resource, string? scope, string? order)
        {
            var response = await _permissaoService.Search(resource, scope, order ?? "asc");
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(ulong id)
        {
            var response = await _permissaoService.GetById(id);
            return Ok(response);
        }

        [HttpPatch("update/{id}")]
        public async Task<IActionResult> Update(ulong id, UpdatePermissaoRequest request)
        {
            request.Id = id;
            await _permissaoService.Update(request);
            return NoContent();
        }

        [HttpDelete("disable")]
        public async Task<IActionResult> Disable(ulong id)
        {
            await _permissaoService.Disable(id);
            return NoContent();
        }
    }
}
