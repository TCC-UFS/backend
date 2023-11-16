using PortalLegisAmbiental.Domain.Dtos.Requests;
using PortalLegisAmbiental.Domain.Dtos.Responses;

namespace PortalLegisAmbiental.Application.Services.Interfaces
{
    public interface IPermissaoService
    {
        Task Add(AddPermissaoRequest permissaoRequest);
        Task<List<PermissaoResponse>> SearchByResource(string? recurso);
        Task<List<PermissaoResponse>> SearchByScope(string scope);
        Task<PermissaoResponse> GetById(ulong id);
        Task<List<PermissaoResponse>> GetAll();
        Task Update(UpdatePermissaoRequest permissaoRequest);
        Task Disable(ulong id);
    }
}
