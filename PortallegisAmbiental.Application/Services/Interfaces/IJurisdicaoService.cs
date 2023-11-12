using PortalLegisAmbiental.Domain.Dtos.Requests;
using PortalLegisAmbiental.Domain.Dtos.Responses;

namespace PortalLegisAmbiental.Application.Services.Interfaces
{
    public interface IJurisdicaoService
    {
        Task Add(AddJurisdicaoRequest jurisdicaoRequest);
        Task<List<JurisdicaoResponse>> GetAll();
        Task<List<JurisdicaoResponse>> SearchByState(string? estado);
        Task<JurisdicaoResponse> GetBySigla(string sigla);
        Task<List<JurisdicaoResponse>> SearchByAmbito(string ambito);
        Task<JurisdicaoResponse> GetById(ulong id);
        Task Update(UpdateJurisdicaoRequest jurisdicaoRequest);
        Task Disable(ulong id);
    }
}
