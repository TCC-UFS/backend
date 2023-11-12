using PortalLegisAmbiental.Domain.Entities;
using PortalLegisAmbiental.Domain.Enums;

namespace PortalLegisAmbiental.Domain.IRepositories
{
    public interface IJurisdicaoRepository : IRepository
    {
        Task Add(Jurisdicao jurisdicao);
        Task<Jurisdicao?> GetById(ulong id, bool noTracking = false);
        Task<List<Jurisdicao>> SearchByState(string estado, bool noTracking = false);
        Task<Jurisdicao?> GetBySigla(string sigla, bool noTracking = false);
        Task<List<Jurisdicao>> SearchByAmbito(EAmbitoType ambito, bool noTracking = false);
        Task<List<Jurisdicao>> GetAll();
        Task<bool> Exists(Jurisdicao jurisdicao);
        Task Disable(Jurisdicao jurisdicao);
    }
}
