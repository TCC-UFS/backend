using PortalLegisAmbiental.Domain.Dtos.Requests;
using PortalLegisAmbiental.Domain.Dtos.Responses;

namespace PortalLegisAmbiental.Application.Services.Interfaces
{
    public interface IAtoService
    {
        Task<AtoResponse> Add(AddAtoRequest atoRequest);
        Task<List<AtoResponse>> GetAll();
        Task<AtoResponse?> GetById(ulong id);
        Task<AtoResponse?> GetByNumber(string number);
        Task<List<AtoResponse>> Search(string? numero = null, string? tipo = null, string? jurisdicao = null, string order = "desc");
        Task<List<AtoResponse>> SearchByNumber(string number);
        Task<List<AtoResponse>> SearchByTipo(string tipoAto);
        Task<List<AtoResponse>> SearchByJurisdicao(string jurisdicao);
        Task<AtoResponse> Update(UpdateAtoRequest atoRequest);
        Task Disable(ulong id);
    }
}
