using Microsoft.EntityFrameworkCore;
using PortalLegisAmbiental.Domain.Entities;
using PortalLegisAmbiental.Domain.IRepositories;
using PortalLegisAmbiental.Domain.Seedwork;

namespace PortalLegisAmbiental.Infrastructure.MySQL.Repositories
{
    public class GrupoRepository : IGrupoRepository
    {
        private readonly EfDbContext _dbContext;
        public IUnitOfWork UnitOfWork => _dbContext;

        public GrupoRepository(EfDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Grupo grupo)
        {
            await _dbContext.AddAsync(grupo);
        }

        public async Task<Grupo> AddAndReturn(Grupo grupo)
        {
            var added = await _dbContext.AddAsync(grupo);
            return added.Entity;
        }

        public async Task<List<Grupo>> GetAll(bool includePermissions = false)
        {
            if (includePermissions) 
                return await _dbContext.Grupos
                    .AsNoTracking()
                    .Include(grupo => grupo.Permissoes)
                    .Where(grupo => grupo.IsActive)
                    .ToListAsync();
            else
                return await _dbContext.Grupos
                    .AsNoTracking()
                    .Where(grupo => grupo.IsActive)
                    .ToListAsync();
        }

        public async Task<Grupo?> GetById(ulong id, bool noTracking = false, bool includePermissions = false)
        {
            if (includePermissions)
            {
                if (noTracking)
                    return await _dbContext.Grupos
                        .AsNoTracking()
                        .Include(grupo => grupo.Permissoes)
                        .FirstOrDefaultAsync(grupo => grupo.Id.Equals(id) && grupo.IsActive);
                else
                    return await _dbContext.Grupos
                        .Include(grupo => grupo.Permissoes)
                        .FirstOrDefaultAsync(grupo => grupo.Id.Equals(id) && grupo.IsActive);
            }
            else
            {
                if (noTracking)
                    return await _dbContext.Grupos
                        .AsNoTracking()
                        .FirstOrDefaultAsync(grupo => grupo.Id.Equals(id) && grupo.IsActive);
                else
                    return await _dbContext.Grupos
                        .FirstOrDefaultAsync(grupo => grupo.Id.Equals(id) && grupo.IsActive);
            }
        }

        public async Task<Grupo?> GetByName(string name, bool noTracking = false)
        {
            if (noTracking)
                return await _dbContext.Grupos
                    .AsNoTracking()
                    .FirstOrDefaultAsync(grupo => grupo.Nome.ToLower().Equals(name.ToLower()) && grupo.IsActive);
            else
                return await _dbContext.Grupos
                    .FirstOrDefaultAsync(grupo => grupo.Nome.ToLower().Equals(name.ToLower()) && grupo.IsActive);
        }

        public async Task<List<Grupo>> SearchByName(string name, bool noTracking = false, bool includePermissions = false)
        {
            if (includePermissions)
            {
                if (noTracking)
                    return await _dbContext.Grupos
                        .AsNoTracking()
                        .Include(grupo => grupo.Permissoes)
                        .Where(grupo => grupo.Nome.StartsWith(name) && grupo.IsActive)
                        .ToListAsync();
                else
                    return await _dbContext.Grupos
                        .Include(grupo => grupo.Permissoes)
                        .Where(grupo => grupo.Nome.StartsWith(name) && grupo.IsActive)
                        .ToListAsync();
            }
            else
            {
                if (noTracking)
                    return await _dbContext.Grupos
                        .AsNoTracking()
                        .Where(grupo => grupo.Nome.StartsWith(name) && grupo.IsActive)
                        .ToListAsync();
                else
                    return await _dbContext.Grupos
                        .Where(grupo => grupo.Nome.StartsWith(name) && grupo.IsActive)
                        .ToListAsync();
            }
        }

        public async Task<bool> Exists(Grupo grupo)
        {
            return await _dbContext.Grupos
                .CountAsync(group =>
                    group.Nome.ToLower().Equals(grupo.Nome.ToLower())
                    && grupo.IsActive) > 0;
        }

        public async Task Disable(Grupo grupo)
        {
            grupo.Disable();
            await _dbContext.Usuarios
                .Where(user => user.Grupos.Find(guser => guser.Id.Equals(user.Id)) != null)
                .ForEachAsync(user =>
                {
                    var guser = user.Grupos.Find(group => group.Id.Equals(user.Id));
                    if (guser != null)
                        user.RemoveGroup(guser);
                });
        }
    }
}
