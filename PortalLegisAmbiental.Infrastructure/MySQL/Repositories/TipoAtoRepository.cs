using Microsoft.EntityFrameworkCore;
using PortalLegisAmbiental.Domain.Entities;
using PortalLegisAmbiental.Domain.IRepositories;
using PortalLegisAmbiental.Domain.Seedwork;

namespace PortalLegisAmbiental.Infrastructure.MySQL.Repositories
{
    public class TipoAtoRepository : ITipoAtoRepository
    {
        private readonly EfDbContext _dbContext;
        public IUnitOfWork UnitOfWork => _dbContext;

        public TipoAtoRepository(EfDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(TipoAto tipoAto)
        {
            await _dbContext.AddAsync(tipoAto);
        }

        public async Task<List<TipoAto>> GetAll()
        {
            return await _dbContext.TiposAtos
                .AsNoTracking()
                .Where(tipoAto => tipoAto.IsActive)
                .ToListAsync();
        }

        public async Task<TipoAto?> GetById(ulong id, bool noTracking = false)
        {
            if (noTracking)
                return await _dbContext.TiposAtos
                    .AsNoTracking()
                    .FirstOrDefaultAsync(tipoAto => tipoAto.Id.Equals(id) && tipoAto.IsActive);
            else
                return await _dbContext.TiposAtos
                    .FirstOrDefaultAsync(tipoAto => tipoAto.Id.Equals(id) && tipoAto.IsActive);
        }

        public async Task<List<TipoAto>> Search(string? name, string order, bool noTracking = false)
        {
            var tiposAtos = _dbContext.TiposAtos.Where(tipoAto => tipoAto.IsActive);

            if (noTracking)
                tiposAtos = tiposAtos.AsNoTracking();

            if (!string.IsNullOrEmpty(name))
                tiposAtos = tiposAtos.Where(tipoAto => tipoAto.Nome.StartsWith(name));

            if (order.ToLower().Equals("desc"))
                tiposAtos = tiposAtos.OrderByDescending(tipoAto => tipoAto.Nome);
            else
                tiposAtos = tiposAtos.OrderBy(tipoAto => tipoAto.Nome);

            return await tiposAtos.ToListAsync();
        }

        public async Task<bool> Exists(TipoAto tipoAto)
        {
            return await _dbContext.TiposAtos
                .CountAsync(tipo => tipo.Nome.Equals(tipoAto.Nome) && tipo.IsActive) > 0;
        }

        public async Task<bool> Exists(ulong tipoAtoId)
        {
            return await _dbContext.TiposAtos
                .CountAsync(tipo => tipo.Id.Equals(tipoAtoId) && tipo.IsActive) > 0;
        }

        public async Task Disable(TipoAto tipoAto)
        {
            tipoAto.Disable();
            await _dbContext.Atos
                .Where(ato => ato.TipoAtoId.Equals(tipoAto.Id) && ato.IsActive)
                .ForEachAsync(ato => ato.Disable());
        }
    }
}
