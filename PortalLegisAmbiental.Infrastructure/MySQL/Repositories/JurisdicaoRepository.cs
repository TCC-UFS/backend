using Microsoft.EntityFrameworkCore;
using PortalLegisAmbiental.Domain.Entities;
using PortalLegisAmbiental.Domain.Enums;
using PortalLegisAmbiental.Domain.IRepositories;
using PortalLegisAmbiental.Domain.Seedwork;

namespace PortalLegisAmbiental.Infrastructure.MySQL.Repositories
{
    public class JurisdicaoRepository : IJurisdicaoRepository
    {
        private readonly EfDbContext _dbContext;
        public IUnitOfWork UnitOfWork => _dbContext;

        public JurisdicaoRepository(EfDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Jurisdicao jurisdicao)
        {
            await _dbContext.AddAsync(jurisdicao);
        }

        public async Task<Jurisdicao?> GetById(ulong id, bool noTracking = false)
        {
            if (noTracking)
                return await _dbContext.Jurisdicoes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(jurisdicao => jurisdicao.Id.Equals(id) && jurisdicao.IsActive);
            else
                return await _dbContext.Jurisdicoes
                    .FirstOrDefaultAsync(jurisdicao => jurisdicao.Id.Equals(id) && jurisdicao.IsActive);
        }

        public async Task<List<Jurisdicao>> Search(string? state = null, string? sigla = null, EAmbitoType? ambito = null, string order = "desc", bool noTracking = false)
        {
            var jurisdicoes = _dbContext.Jurisdicoes.Where(jur => jur.IsActive);

            if (noTracking)
                jurisdicoes = jurisdicoes.AsNoTracking();

            if (!string.IsNullOrEmpty(state))
                jurisdicoes = jurisdicoes.Where(jur => jur.Estado != null && jur.Estado.StartsWith(state));

            if (!string.IsNullOrEmpty(sigla))
                jurisdicoes = jurisdicoes.Where(jur => jur.Sigla.Equals(sigla));

            if (ambito.HasValue)
                jurisdicoes = jurisdicoes.Where(jur => jur.Ambito.Equals(ambito.Value));

            if (order.ToLower().Equals("desc"))
                jurisdicoes = jurisdicoes.OrderByDescending(jur => jur.Ambito).ThenByDescending(jur => jur.Sigla);
            else
                jurisdicoes = jurisdicoes.OrderBy(jur => jur.Ambito).ThenBy(jur => jur.Sigla);

            return await jurisdicoes.ToListAsync();
        }

        public async Task<List<Jurisdicao>> GetAll()
        {
            return await _dbContext.Jurisdicoes
                .AsNoTracking()
                .Where(jurisdicao => jurisdicao.IsActive)
                .ToListAsync();
        }

        public async Task<bool> Exists(Jurisdicao jurisdicao)
        {
            return await _dbContext.Jurisdicoes
                .CountAsync(jur =>
                    jur.Ambito.Equals(jurisdicao.Ambito)
                    && (jur.Estado == null || (jur.Estado != null
                        && jur.Estado.Equals(jurisdicao.Estado)))
                    && jur.Sigla.Equals(jurisdicao.Sigla)
                    && jur.IsActive) > 0;
        }

        public async Task<bool> Exists(ulong jurisdicaoId)
        {
            return await _dbContext.Jurisdicoes
                .CountAsync(jur =>
                    jur.Id.Equals(jurisdicaoId)
                    && jur.IsActive) > 0;
        }

        public async Task Disable(Jurisdicao jurisdicao)
        {
            jurisdicao.Disable();
            await _dbContext.Atos
                .Where(ato => ato.JurisdicaoId.Equals(jurisdicao.Id) && ato.IsActive)
                .ForEachAsync(ato => ato.Disable());
        }
    }
}
