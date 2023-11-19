using PortalLegisAmbiental.Domain.Entities;
using PortalLegisAmbiental.Domain.Enums;

namespace PortalLegisAmbiental.Domain.IRepositories
{
    public interface IJurisdicaoRepository : IRepository
    {
        Task Add(Jurisdicao jurisdicao);
        Task<Jurisdicao?> GetById(ulong id, bool noTracking = false);
        Task<List<Jurisdicao>> Search(string? state = null, string? sigla = null, EAmbitoType? ambito = null, string order = "desc", bool noTracking = false);
        Task<List<Jurisdicao>> GetAll();
        Task<bool> Exists(Jurisdicao jurisdicao);
        Task<bool> Exists(ulong jurisdicaoId);
        Task Disable(Jurisdicao jurisdicao);
    }
}
