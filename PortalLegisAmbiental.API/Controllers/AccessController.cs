using Microsoft.AspNetCore.Mvc;
using PortalLegisAmbiental.API.Controllers.Base;
using PortalLegisAmbiental.Application.Services.Interfaces;
using PortalLegisAmbiental.Domain.Dtos.Requests;

namespace PortalLegisAmbiental.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/accounts")]
    public class AccessController : BaseController
    {
        private readonly IAccessService _accessService;
        private readonly IUsuarioService _usuarioService;

        public AccessController(IAccessService accessService, IUsuarioService usuarioService)
        {
            _accessService = accessService;
            _usuarioService = usuarioService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest userLogin)
        {
            var accessToken = await _accessService.AuthenticateUser(userLogin);
            return Ok(accessToken);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(AddUsuarioRequest userRequest)
        {
            await _usuarioService.Register(userRequest);
            return Ok();
        }
    }
}
