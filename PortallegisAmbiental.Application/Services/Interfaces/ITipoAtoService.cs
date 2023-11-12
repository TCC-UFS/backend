using PortalLegisAmbiental.Domain.Dtos.Requests;
using PortalLegisAmbiental.Domain.Dtos.Responses;

namespace PortalLegisAmbiental.Application.Services.Interfaces
{
    public interface ITipoAtoService
    {
        Task Add(AddTipoAtoRequest tipoAtoRequest);
        Task<List<TipoAtoResponse>> SearchByName(string? name);
        Task<TipoAtoResponse> GetById(ulong id);
        Task Update(UpdateTipoAtoRequest tipoAtoRequest);
        Task Disable(ulong id);
    }
}
