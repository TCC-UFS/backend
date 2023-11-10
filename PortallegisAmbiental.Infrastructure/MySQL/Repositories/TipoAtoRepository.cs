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

        public async Task<List<TipoAto>> SearchByName(string name, bool noTracking = false)
        {
            if (noTracking)
                return await _dbContext.TiposAtos
                    .AsNoTracking()
                    .Where(tipoAto => tipoAto.Nome.Contains(name) && tipoAto.IsActive)
                    .ToListAsync();
            else
                return await _dbContext.TiposAtos
                    .Where(tipoAto => tipoAto.Nome.Contains(name) && tipoAto.IsActive)
                    .ToListAsync();
        }

        public async Task Disable(TipoAto tipoAto)
        {
            tipoAto.Disable();
            await _dbContext.Atos
                .Where(ato => ato.TipoAtoId.Equals(tipoAto.Id) && ato.IsActive)
                .ForEachAsync(ato => ato.Disable());

            UnitOfWork.SaveChanges();
        }
    }
}
