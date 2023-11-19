using PortalLegisAmbiental.Domain.Entities;

namespace PortalLegisAmbiental.Domain.IRepositories
{
    public interface IUsuarioRepository : IRepository
    {
        Task Add(Usuario usuario);
        Task<List<Usuario>> GetAll(bool includeGroups = false);
        Task<Usuario?> GetById(ulong id, bool noTracking = false, bool includeGroups = false);
        Task<Usuario?> GetByEmail(string email, bool noTracking = false, bool includeGroups = false);
        Task<List<Usuario>> Search(string? name, string? email, string order, bool noTracking = false, bool includeGroups = false);
        Task<bool> Exists(Usuario usuario);
        Task<bool> Exists(ulong usuarioId);
    }
}
