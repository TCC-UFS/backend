using PortalLegisAmbiental.Domain.Dtos.Requests;
using PortalLegisAmbiental.Domain.Dtos.Responses;

namespace PortalLegisAmbiental.Application.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task Add(AddUsuarioRequest usuarioRequest);
        Task Register(AddUsuarioRequest usuarioRequest);
        Task<List<UsuarioResponse>> SearchByName(string? name);
        Task<List<UsuarioResponse>> SearchByEmail(string? email);
        Task<UsuarioResponse> GetById(ulong id);
        Task<List<UsuarioResponse>> GetAll();
        Task Update(UpdateUsuarioRequest usuarioRequest);
        Task AddGroup(UserGroupRequest request);
        Task RemoveGroup(UserGroupRequest request);
        Task Disable(ulong id);
    }
}
