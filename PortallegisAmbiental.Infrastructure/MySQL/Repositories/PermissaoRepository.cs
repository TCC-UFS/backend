using Microsoft.EntityFrameworkCore;
using PortalLegisAmbiental.Domain.Entities;
using PortalLegisAmbiental.Domain.Enums;
using PortalLegisAmbiental.Domain.IRepositories;
using PortalLegisAmbiental.Domain.Seedwork;

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

        public async Task<List<Permissao>> GetAll()
        {
            return await _dbContext.Permissoes
                .AsNoTracking()
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

        public async Task<List<Permissao>> SearchByResource(string recurso, bool noTracking = false)
        {
            if (noTracking)
                return await _dbContext.Permissoes
                    .AsNoTracking()
                    .Where(permissao => permissao.Recurso.StartsWith(recurso) && permissao.IsActive)
                    .ToListAsync();
            else
                return await _dbContext.Permissoes
                    .Where(permissao => permissao.Recurso.StartsWith(recurso) && permissao.IsActive)
                    .ToListAsync();
        }

        public async Task<List<Permissao>> SearchByScope(EScopeType scope, bool noTracking = false)
        {
            if (noTracking)
                return await _dbContext.Permissoes
                    .AsNoTracking()
                    .Where(permissao => permissao.Scope.Equals(scope) && permissao.IsActive)
                    .ToListAsync();
            else
                return await _dbContext.Permissoes
                    .Where(permissao => permissao.Scope.Equals(scope) && permissao.IsActive)
                    .ToListAsync();
        }

        public async Task<bool> Exists(Permissao permissao)
        {
            return await _dbContext.Permissoes
                .CountAsync(perm =>
                    perm.Scope.Equals(permissao.Scope)
                    && perm.Recurso.Equals(permissao.Recurso)
                    && permissao.IsActive) > 0;
        }

        public async Task Disable(Permissao permissao)
        {
            permissao.Disable();
            await _dbContext.Grupos
                .Where(grupo => grupo.Permissoes.Find(gperm => gperm.Id.Equals(permissao.Id)) != null)
                .ForEachAsync(grupo =>
                {
                    var gperm = grupo.Permissoes.Find(gperm => gperm.Id.Equals(permissao.Id));
                    if (gperm != null)
                        grupo.RemovePermission(gperm);
                });
        }
    }
}
