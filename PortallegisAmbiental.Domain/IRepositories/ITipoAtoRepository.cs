using PortalLegisAmbiental.Domain.Entities;

namespace PortalLegisAmbiental.Domain.IRepositories
{
    public interface ITipoAtoRepository : IRepository
    {
        Task Add(TipoAto tipoAto);
        Task<TipoAto?> GetById(ulong id, bool noTracking = false);
        Task<List<TipoAto>> SearchByName(string name, bool noTracking = false);
        Task<bool> Exists(TipoAto tipoAto);
        Task Disable(TipoAto tipoAto);
    }
}
