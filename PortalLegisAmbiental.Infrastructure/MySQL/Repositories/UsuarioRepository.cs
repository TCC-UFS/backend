using Microsoft.EntityFrameworkCore;
using PortalLegisAmbiental.Domain.Entities;
using PortalLegisAmbiental.Domain.IRepositories;
using PortalLegisAmbiental.Domain.Seedwork;

namespace PortalLegisAmbiental.Infrastructure.MySQL.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly EfDbContext _dbContext;
        public IUnitOfWork UnitOfWork => _dbContext;

        public UsuarioRepository(EfDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Usuario usuario)
        {
            await _dbContext.AddAsync(usuario);
        }

        public async Task<List<Usuario>> GetAll(bool includeGroups = false)
        {
            if (includeGroups)
                return await _dbContext.Usuarios
                    .AsNoTracking()
                    .Include(user => user.Grupos)
                    .ThenInclude(grupo => grupo.Permissoes)
                    .Where(user => user.IsActive)
                    .ToListAsync();
            else
                return await _dbContext.Usuarios
                    .AsNoTracking()
                    .Where(user => user.IsActive)
                    .ToListAsync();
        }

        public async Task<Usuario?> GetById(ulong id, bool noTracking = false, bool includeGroups = false)
        {
            if (includeGroups)
            {
                if (noTracking)
                    return await _dbContext.Usuarios
                        .AsNoTracking()
                        .Include(user => user.Grupos)
                        .ThenInclude(grupo => grupo.Permissoes)
                        .FirstOrDefaultAsync(usuario => usuario.Id.Equals(id) && usuario.IsActive);
                else
                    return await _dbContext.Usuarios
                        .Include(user => user.Grupos)
                        .ThenInclude(grupo => grupo.Permissoes)
                        .FirstOrDefaultAsync(usuario => usuario.Id.Equals(id) && usuario.IsActive);
            }
            else
            {
                if (noTracking)
                    return await _dbContext.Usuarios
                        .AsNoTracking()
                        .FirstOrDefaultAsync(usuario => usuario.Id.Equals(id) && usuario.IsActive);
                else
                    return await _dbContext.Usuarios
                        .FirstOrDefaultAsync(usuario => usuario.Id.Equals(id) && usuario.IsActive);
            }
        }

        public async Task<Usuario?> GetByEmail(string email, bool noTracking = false, bool includeGroups = false)
        {
            if (includeGroups)
            {
                if (noTracking)
                    return await _dbContext.Usuarios
                        .AsNoTracking()
                        .Include(user => user.Grupos)
                        .ThenInclude(grupo => grupo.Permissoes)
                        .FirstOrDefaultAsync(usuario => usuario.Email.Equals(email) && usuario.IsActive);
                else
                {
                    return await _dbContext.Usuarios
                        .Include(user => user.Grupos)
                        .ThenInclude(grupo => grupo.Permissoes)
                        .FirstOrDefaultAsync(usuario => usuario.Email.Equals(email) && usuario.IsActive);
                }
            }
            else
            {
                if (noTracking)
                    return await _dbContext.Usuarios
                        .AsNoTracking()
                        .FirstOrDefaultAsync(usuario => usuario.Email.Equals(email) && usuario.IsActive);
                else
                    return await _dbContext.Usuarios
                        .FirstOrDefaultAsync(usuario => usuario.Email.Equals(email) && usuario.IsActive);
            }
        }

        public async Task<List<Usuario>> Search(string? name, string? email, string order, bool noTracking = false, bool includeGroups = false)
        {
            var users = _dbContext.Usuarios.Where(user => user.IsActive);

            if (noTracking)
                users = users.AsNoTracking();

            if (includeGroups)
                users = users.Include(user => user.Grupos).ThenInclude(grupo => grupo.Permissoes);

            if (!string.IsNullOrEmpty(name))
                users = users.Where(user => user.Nome.StartsWith(name));

            if (!string.IsNullOrEmpty(email))
                users = users.Where(user => user.Nome.StartsWith(email));

            if (order.ToLower().Equals("desc"))
                users = users.OrderByDescending(user => user.Nome);
            else
                users = users.OrderBy(user => user.Nome);

            return await users.ToListAsync();
        }

        public async Task<bool> Exists(Usuario usuario)
        {
            return await _dbContext.Usuarios
                .CountAsync(user => user.Email.Equals(usuario.Email) && user.IsActive) > 0;
        }

        public async Task<bool> Exists(ulong usuarioId)
        {
            return await _dbContext.Usuarios
                .CountAsync(user => user.Id.Equals(usuarioId) && user.IsActive) > 0;
        }
    }
}
