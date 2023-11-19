using Microsoft.EntityFrameworkCore;
using PortalLegisAmbiental.Domain.Entities;
using PortalLegisAmbiental.Domain.Enums;
using PortalLegisAmbiental.Domain.IRepositories;
using PortalLegisAmbiental.Domain.Seedwork;
using System.Diagnostics.SymbolStore;

namespace PortalLegisAmbiental.Infrastructure.MySQL.Repositories
{
    public class PermissaoRepository : IPermissaoRepository
    {
        private readonly EfDbContext _dbContext;
        public IUnitOfWork UnitOfWork => _dbContext;

        public PermissaoRepository(EfDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Permissao permissao)
        {
            await _dbContext.AddAsync(permissao);
        }

        public async Task<Permissao> AddAndReturn(Permissao permissao)
        {
            var perm = await _dbContext.AddAsync(permissao);
            return perm.Entity;
        }

        public async Task<List<Permissao>> GetAll()
        {
            return await _dbContext.Permissoes
                .AsNoTracking()
                .Where(permissao => permissao.IsActive)
                .ToListAsync();
        }

        public async Task<Permissao?> GetById(ulong id, bool noTracking = false)
        {
            if (noTracking)
                return await _dbContext.Permissoes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(permissao => permissao.Id.Equals(id) && permissao.IsActive);
            else
                return await _dbContext.Permissoes
                    .FirstOrDefaultAsync(permissao => permissao.Id.Equals(id) && permissao.IsActive);
        }

        public async Task<List<Permissao>> Search(string? resource, EScopeType? scope, string order, bool noTracking = false)
        {
            var permissoes = _dbContext.Permissoes.Where(perm => perm.IsActive);

            if (noTracking)
                permissoes = permissoes.AsNoTracking();

            if (!string.IsNullOrEmpty(resource))
                permissoes = permissoes.Where(perm => perm.Recurso.StartsWith(resource));

            if (scope.HasValue)
                permissoes = permissoes.Where(perm => perm.Scope.Equals(scope.Value));

            if (order.ToLower().Equals("desc"))
                permissoes = permissoes.OrderByDescending(perm => perm.Scope).ThenByDescending(perm => perm.Recurso);
            else
                permissoes = permissoes.OrderBy(perm => perm.Scope).ThenBy(perm => perm.Recurso);

            return await permissoes.ToListAsync();
        }

        public async Task<Permissao?> GetByResourceAndScope(string recurso, EScopeType scope, bool noTracking = false)
        {
            if (noTracking)
                return await _dbContext.Permissoes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(permissao =>
                        permissao.Recurso.Equals(recurso)
                        && permissao.Scope.Equals(scope)
                        && permissao.IsActive);
            else
                return await _dbContext.Permissoes
                    .FirstOrDefaultAsync(permissao =>
                        permissao.Recurso.Equals(recurso)
                        && permissao.Scope.Equals(scope)
                        && permissao.IsActive);
        }

        public async Task<bool> Exists(Permissao permissao)
        {
            return await _dbContext.Permissoes
                .CountAsync(perm =>
                    perm.Scope.Equals(permissao.Scope)
                    && perm.Recurso.Equals(permissao.Recurso)
                    && perm.IsActive) > 0;
        }

        public async Task Disable(Permissao permissao)
        {
            permissao.Disable();
            await _dbContext.Grupos.Include(grupo => grupo.Permissoes)
                .Where(grupo => grupo.Permissoes.Any(gperm => gperm.Id.Equals(permissao.Id)))
                .ForEachAsync(grupo =>
                {
                    var gperm = grupo.Permissoes.Find(gperm => gperm.Id.Equals(permissao.Id));
                    if (gperm != null)
                        grupo.RemovePermission(gperm);
                });
        }
    }
}
