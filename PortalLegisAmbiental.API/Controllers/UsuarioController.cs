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
    [Route("api/v{version:apiVersion}/users")]
    public class UsuarioController : BaseController
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddUsuarioRequest request)
        {
            await _usuarioService.Add(request);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string? name, string? email, string? order)
        {
            var response = await _usuarioService.Search(name, email, order ?? "asc");
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(ulong id)
        {
            var response = await _usuarioService.GetById(id);
            return Ok(response);
        }

        [HttpPatch("update/{id}")]
        public async Task<IActionResult> Update(ulong id, UpdateUsuarioRequest request)
        {
            request.Id = id;
            await _usuarioService.Update(request);
            return NoContent();
        }

        [HttpPost("{id}/add-groups")]
        public async Task<IActionResult> AddGroup(ulong id, UserGroupRequest request)
        {
            request.UserId = id;
            await _usuarioService.AddGroup(request);
            return NoContent();
        }

        [HttpPost("{id}/remove-groups")]
        public async Task<IActionResult> RemoveGroup(ulong id, UserGroupRequest request)
        {
            request.UserId = id;
            await _usuarioService.RemoveGroup(request);
            return NoContent();
        }

        [HttpDelete("disable")]
        public async Task<IActionResult> Disable(ulong id)
        {
            await _usuarioService.Disable(id);
            return NoContent();
        }
    }
}
