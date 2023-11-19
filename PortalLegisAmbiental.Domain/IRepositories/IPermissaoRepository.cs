using PortalLegisAmbiental.Domain.Entities;
using PortalLegisAmbiental.Domain.Enums;

namespace PortalLegisAmbiental.Domain.IRepositories
{
    public interface IPermissaoRepository : IRepository
    {
        Task Add(Permissao permissao);
        Task<Permissao> AddAndReturn(Permissao permissao);
        Task<List<Permissao>> GetAll();
        Task<List<Permissao>> Search(string? resource, EScopeType? scope, string order, bool noTracking = false);
        Task<Permissao?> GetById(ulong id, bool noTracking = false);
        Task<Permissao?> GetByResourceAndScope(string recurso, EScopeType scope, bool noTracking = false);
        Task<bool> Exists(Permissao permissao);
        Task Disable(Permissao permissao);
    }
}
