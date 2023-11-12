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
                        .FirstOrDefaultAsync(usuario => usuario.Id.Equals(id) && usuario.IsActive);
                else
                    return await _dbContext.Usuarios
                        .Include(user => user.Grupos)
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
                        .FirstOrDefaultAsync(usuario => usuario.Email.Equals(email) && usuario.IsActive);
                else
                    return await _dbContext.Usuarios
                        .Include(user => user.Grupos)
                        .FirstOrDefaultAsync(usuario => usuario.Email.Equals(email) && usuario.IsActive);
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

        public async Task<List<Usuario>> SearchByName(string name, bool noTracking = false, bool includeGroups = false)
        {
            if (includeGroups)
            {
                if (noTracking)
                    return await _dbContext.Usuarios
                        .AsNoTracking()
                        .Include(user => user.Grupos)
                        .Where(usuario => usuario.Nome.StartsWith(name) && usuario.IsActive)
                        .ToListAsync();
                else
                    return await _dbContext.Usuarios
                        .Include(user => user.Grupos)
                        .Where(usuario => usuario.Nome.StartsWith(name) && usuario.IsActive)
                        .ToListAsync();
            }
            else
            {
                if (noTracking)
                    return await _dbContext.Usuarios
                        .AsNoTracking()
                        .Where(usuario => usuario.Nome.StartsWith(name) && usuario.IsActive)
                        .ToListAsync();
                else
                    return await _dbContext.Usuarios
                        .Where(usuario => usuario.Nome.StartsWith(name) && usuario.IsActive)
                        .ToListAsync();
            }
        }

        public async Task<List<Usuario>> SearchByEmail(string email, bool noTracking = false, bool includeGroups = false)
        {
            if (includeGroups)
            {
                if (noTracking)
                    return await _dbContext.Usuarios
                        .AsNoTracking()
                        .Include(user => user.Grupos)
                        .Where(usuario => usuario.Email.StartsWith(email) && usuario.IsActive)
                        .ToListAsync();
                else
                    return await _dbContext.Usuarios
                        .Include(user => user.Grupos)
                        .Where(usuario => usuario.Email.StartsWith(email) && usuario.IsActive)
                        .ToListAsync();
            }
            else
            {
                if (noTracking)
                    return await _dbContext.Usuarios
                        .AsNoTracking()
                        .Where(usuario => usuario.Email.StartsWith(email) && usuario.IsActive)
                        .ToListAsync();
                else
                    return await _dbContext.Usuarios
                        .Where(usuario => usuario.Email.StartsWith(email) && usuario.IsActive)
                        .ToListAsync();
            }
        }

        public async Task<bool> Exists(Usuario usuario)
        {
            return await _dbContext.Usuarios
                .CountAsync(user => user.Email.Equals(usuario.Email) && user.IsActive) > 0;
        }
    }
}
