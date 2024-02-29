using PortalLegisAmbiental.Domain.Dtos.Responses;
using PortalLegisAmbiental.Domain.Entities;
using PortalLegisAmbiental.Domain.Enums;

namespace PortalLegisAmbiental.Domain.IRepositories
{
    public interface IAtoRepository : IRepository
    {
        Task<Ato> Add(Ato ato);
        Task<StatsResponse> GetStats();
        Task<List<Ato>> GetAll(bool includeJurisdicao = false, bool includeCreated = false, bool includeTipo = false, bool tracking = false, string order = "desc");
        Task<Ato?> GetById(ulong id, bool includeJurisdicao = false, bool includeCreated = false, bool includeTipo = false, bool tracking = false);
        Task<Ato?> GetByNumber(string number, bool includeJurisdicao = false, bool includeCreated = false, bool includeTipo = false, bool tracking = false);
        Task<List<Ato>> Search(string? numero = null, string? tipo = null, string? jurisdicao = null, EAmbitoType? ambito = null,
                                            bool includeJurisdicao = false, bool includeCreated = false, bool includeTipo = false, bool tracking = false,
                                            string order = "desc", ulong[] ids = null!);
        Task<List<Ato>> SearchByNumber(string number, bool includeJurisdicao = false, bool includeCreated = false, bool includeTipo = false, bool tracking = false);
        Task<List<Ato>> SearchByTipo(string tipoAto, bool includeJurisdicao = false, bool includeCreated = false, bool tracking = false);
        Task<List<Ato>> SearchByJurisdicao(string jurisdicao, bool includeCreated = false, bool includeTipo = false, bool tracking = false);
        Task<List<Ato>> SearchByJurisdicao(EAmbitoType ambito, bool includeCreated = false, bool includeTipo = false, bool tracking = false);
        Task<bool> Exists(Ato ato);
    }
}
