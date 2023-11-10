using PortalLegisAmbiental.Domain.Entities;

namespace PortalLegisAmbiental.Domain.IRepositories
{
    public interface IUsuarioRepository : IRepository
    {
        Task Add(Usuario usuario);
        Task<Usuario?> GetById(ulong id, bool noTracking = false);
        Task<List<Usuario>> SearchByName(string name, bool noTracking = false);
        Task<List<Usuario>> SearchByEmail(string email, bool noTracking = false);
    }
}
