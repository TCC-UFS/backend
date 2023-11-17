using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalLegisAmbiental.API.Controllers.Base;
using PortalLegisAmbiental.Application.Services.Interfaces;
using PortalLegisAmbiental.Domain.Dtos.Requests;

namespace PortalLegisAmbiental.API.Controllers
{
    [ApiVersion("1")]
    [Authorize]
    [Route("api/v{version:apiVersion}/grupos")]
    public class GrupoController : BaseController
    {
        private readonly IGrupoService _grupoService;

        public GrupoController(IGrupoService grupoService)
        {
            _grupoService = grupoService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddGrupoRequest request)
        {
            await _grupoService.Add(request);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _grupoService.GetAll();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(ulong id)
        {
            var response = await _grupoService.GetById(id);
            return Ok(response);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string? name)
        {
            var response = await _grupoService.SearchByName(name);
            return Ok(response);
        }

        [HttpPatch("update/{id}")]
        public async Task<IActionResult> Update(ulong id, UpdateGrupoRequest request)
        {
            request.Id = id;
            await _grupoService.Update(request);
            return NoContent();
        }

        [HttpPut("{id}/add-permissions")]
        public async Task<IActionResult> AddPermission(ulong id, GroupPermissionRequest request)
        {
            request.GrupoId = id;
            await _grupoService.AddPermission(request);
            return NoContent();
        }

        [HttpPost("{id}/remove-permissions")]
        public async Task<IActionResult> RemovePermission(ulong id, GroupPermissionRequest request)
        {
            request.GrupoId = id;
            await _grupoService.RemovePermission(request);
            return NoContent();
        }

        [HttpPatch("disable")]
        public async Task<IActionResult> Disable(ulong id)
        {
            await _grupoService.Disable(id);
            return NoContent();
        }
    }
}
