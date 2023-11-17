using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalLegisAmbiental.API.Controllers.Base;
using PortalLegisAmbiental.Application.Services.Interfaces;
using PortalLegisAmbiental.Domain.Dtos.Requests;

namespace PortalLegisAmbiental.API.Controllers
{
    [ApiVersion("1")]
    [Authorize]
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
        public async Task<IActionResult> GetAll()
        {
            var response = await _usuarioService.GetAll();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(ulong id)
        {
            var response = await _usuarioService.GetById(id);
            return Ok(response);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string? name, string? email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var response = await _usuarioService.SearchByEmail(email);
                return Ok(response);
            }
            else
            {
                var response = await _usuarioService.SearchByName(name);
                return Ok(response);
            }
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

        [HttpPatch("disable")]
        public async Task<IActionResult> Disable(ulong id)
        {
            await _usuarioService.Disable(id);
            return NoContent();
        }
    }
}
