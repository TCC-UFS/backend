using PortalLegisAmbiental.Domain.Entities;

namespace PortalLegisAmbiental.Domain.IRepositories
{
    public interface IGrupoRepository : IRepository
    {
        Task Add(Grupo grupo);
        Task<Grupo> AddAndReturn(Grupo grupo);
        Task<List<Grupo>> GetAll(bool includePermissions = false);
        Task<Grupo?> GetById(ulong id, bool noTracking = false, bool includePermissions = false);
        Task<Grupo?> GetByName(string name, bool noTracking = false);
        Task<List<Grupo>> Search(string? name, string order, bool noTracking = false, bool includePermissions = false);
        Task<bool> Exists(Grupo grupo);
        Task Disable(Grupo grupo);
    }
}
