using PortalLegisAmbiental.Domain.Dtos.Requests;
using PortalLegisAmbiental.Domain.Dtos.Responses;

namespace PortalLegisAmbiental.Application.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task Add(AddUsuarioRequest usuarioRequest);
        Task Register(AddUsuarioRequest usuarioRequest);
        Task<List<UsuarioResponse>> GetAll();
        Task<UsuarioResponse> GetById(ulong id);
        Task<List<UsuarioResponse>> Search(string? name, string? email, string order);
        Task Update(UpdateUsuarioRequest usuarioRequest);
        Task AddGroup(UserGroupRequest request);
        Task RemoveGroup(UserGroupRequest request);
        Task Disable(ulong id);
    }
}
