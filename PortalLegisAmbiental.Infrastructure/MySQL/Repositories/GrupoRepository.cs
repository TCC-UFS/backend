using Microsoft.EntityFrameworkCore;
using PortalLegisAmbiental.Domain.Entities;
using PortalLegisAmbiental.Domain.IRepositories;
using PortalLegisAmbiental.Domain.Seedwork;
using static System.Formats.Asn1.AsnWriter;

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

        public async Task<List<Grupo>> Search(string? name, string order, bool noTracking = false, bool includePermissions = false)
        {
            var grupos = _dbContext.Grupos.Where(grupo => grupo.IsActive);

            if (noTracking)
                grupos = grupos.AsNoTracking();

            if (includePermissions)
                grupos = grupos.Include(grupo => grupo.Permissoes);

            if (!string.IsNullOrEmpty(name))
                grupos = grupos.Where(grupo => grupo.Nome.StartsWith(name));

            if (order.ToLower().Equals("desc"))
                grupos = grupos.OrderByDescending(grupo => grupo.Nome);
            else
                grupos = grupos.OrderBy(grupo => grupo.Nome);

            return await grupos.ToListAsync();
        }

        public async Task<bool> Exists(Grupo grupo)
        {
            return await _dbContext.Grupos
                .CountAsync(group =>
                    group.Nome.ToLower().Equals(grupo.Nome.ToLower())
                    && group.IsActive) > 0;
        }

        public async Task Disable(Grupo grupo)
        {
            grupo.Disable();
            await _dbContext.Usuarios
                .Include(user => user.Grupos)
                .Where(user => user.Grupos.Any(guser => guser.Id.Equals(grupo.Id)))
                .ForEachAsync(user =>
                {
                    var guser = user.Grupos.Find(group => group.Id.Equals(grupo.Id));
                    if (guser != null)
                        user.RemoveGroup(guser);
                });
        }
    }
}
