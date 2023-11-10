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

        public async Task<Usuario?> GetById(ulong id, bool noTracking = false)
        {
            if (noTracking)
                return await _dbContext.Usuarios
                    .AsNoTracking()
                    .FirstOrDefaultAsync(usuario => usuario.Id.Equals(id) && usuario.IsActive);
            else
                return await _dbContext.Usuarios
                    .FirstOrDefaultAsync(usuario => usuario.Id.Equals(id) && usuario.IsActive);
        }

        public async Task<List<Usuario>> SearchByName(string name, bool noTracking = false)
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

        public async Task<List<Usuario>> SearchByEmail(string email, bool noTracking = false)
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
}
