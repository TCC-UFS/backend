using PortalLegisAmbiental.Domain.Dtos;
using PortalLegisAmbiental.Domain.Dtos.Requests;
using PortalLegisAmbiental.Domain.Dtos.Responses;
using System.Security.Claims;

namespace PortalLegisAmbiental.Application.Services.Interfaces
{
    public interface IAccessService
    {
        Task<AccessTokenDto> AuthenticateUser(LoginRequest userLogin);
        UsuarioResponse GetLoggedUser(ClaimsPrincipal user);
    }
}
