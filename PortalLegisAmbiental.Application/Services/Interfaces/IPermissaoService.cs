using PortalLegisAmbiental.Domain.Dtos.Requests;
using PortalLegisAmbiental.Domain.Dtos.Responses;

namespace PortalLegisAmbiental.Application.Services.Interfaces
{
    public interface IPermissaoService
    {
        Task Add(AddPermissaoRequest permissaoRequest);
        Task<List<PermissaoResponse>> GetAll();
        Task<PermissaoResponse> GetById(ulong id);
        Task<List<PermissaoResponse>> Search(string? resource, string? scope, string order);
        Task Update(UpdatePermissaoRequest permissaoRequest);
        Task Disable(ulong id);
    }
}
