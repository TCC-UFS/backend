using PortalLegisAmbiental.Domain.Dtos.Requests;
using PortalLegisAmbiental.Domain.Dtos.Responses;

namespace PortalLegisAmbiental.Application.Services.Interfaces
{
    public interface IGrupoService
    {
        Task Add(AddGrupoRequest grupoRequest);
        Task<List<GrupoResponse>> Search(string? name, string order);
        Task<GrupoResponse> GetById(ulong id);
        Task<List<GrupoResponse>> GetAll();
        Task Update(UpdateGrupoRequest grupoRequest);
        Task AddPermission(GroupPermissionRequest request);
        Task RemovePermission(GroupPermissionRequest request);
        Task Disable(ulong id);
    }
}
