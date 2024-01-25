using Microsoft.EntityFrameworkCore;
using PortalLegisAmbiental.Domain.Entities;
using PortalLegisAmbiental.Domain.Enums;
using PortalLegisAmbiental.Domain.IRepositories;
using PortalLegisAmbiental.Domain.Seedwork;

namespace PortalLegisAmbiental.Infrastructure.MySQL.Repositories
{
    public class AtoRepository : IAtoRepository
    {
        private readonly EfDbContext _dbContext;
        public IUnitOfWork UnitOfWork => _dbContext;

        public AtoRepository(EfDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Ato> Add(Ato ato)
        {
            var added = await _dbContext.Atos.AddAsync(ato);
            return added.Entity;
        }

        public async Task<List<Ato>> GetAll(bool includeJurisdicao = false, bool includeCreated = false, bool includeTipo = false, bool tracking = false, string order = "desc")
        {
            var atos = _dbContext.Atos.Where(ato => ato.IsActive);

            if (!tracking)
                atos = atos.AsNoTracking();

            if (includeJurisdicao)
                atos = atos.Include(ato => ato.Jurisdicao);

            if (includeCreated)
                atos = atos.Include(ato => ato.CreatedBy);

            if (includeTipo)
                atos = atos.Include(ato => ato.TipoAto);

            if (order.ToLower().Equals("desc"))
                atos = atos.OrderByDescending(ato => ato.DataAto.Year.Equals(1) ? ato.CreatedAt : ato.DataAto);
            else
                atos = atos.OrderBy(ato => ato.DataAto.Year.Equals(1) ? ato.CreatedAt : ato.DataAto);

            return await atos.ToListAsync();
        }

        public async Task<List<Ato>> Search(string? numero = null, string? tipo = null, string? jurisdicao = null, EAmbitoType? ambito = null, 
                                            bool includeJurisdicao = false, bool includeCreated = false, bool includeTipo = false, bool tracking = false,
                                            string order = "desc", ulong[] ids = null!)
        {
            var atos = _dbContext.Atos.Where(ato => ato.IsActive);

            if (!tracking)
                atos = atos.AsNoTracking();

            if (includeJurisdicao)
                atos = atos.Include(ato => ato.Jurisdicao);

            if (includeCreated)
                atos = atos.Include(ato => ato.CreatedBy);

            if (includeTipo)
                atos = atos.Include(ato => ato.TipoAto);

            if (ids != null && ids.Length > 0)
                atos = atos.Where(ato => ids.Contains(ato.Id));

            if (!string.IsNullOrEmpty(numero))
                atos = atos.Where(ato => ato.Numero.ToLower().Replace(".", string.Empty).StartsWith(numero.ToLower().Replace(".", string.Empty)));

            if (!string.IsNullOrEmpty(tipo))
            {
                if (!includeTipo)
                    atos = atos.Include(ato => ato.TipoAto);

                atos = atos.Where(ato => ato.TipoAto.Nome.ToLower().Replace(" ", "-").Equals(tipo.ToLower().Replace(" ", "-")));
            }

            if (!string.IsNullOrEmpty(jurisdicao))
            {
                if (!includeJurisdicao)
                    atos = atos.Include(ato => ato.Jurisdicao);

                atos = atos.Where(ato => ato.Jurisdicao.Sigla.ToLower().Equals(jurisdicao.ToLower())
                    || (ato.Jurisdicao.Estado != null && ato.Jurisdicao.Estado.ToLower().Equals(jurisdicao.ToLower())));
            }

            if (ambito.HasValue)
            {
                if (!includeJurisdicao)
                    atos = atos.Include(ato => ato.Jurisdicao);

                atos = atos.Where(ato => ato.Jurisdicao.Ambito.Equals(ambito.Value));
            }

            if (order.ToLower().Equals("desc"))
                atos = atos.OrderByDescending(ato => ato.DataAto.Year.Equals(1) ? ato.CreatedAt : ato.DataAto);
            else
                atos = atos.OrderBy(ato => ato.DataAto.Year.Equals(1) ? ato.CreatedAt : ato.DataAto);

            return await atos.ToListAsync();
        }

        public async Task<Ato?> GetById(ulong id, bool includeJurisdicao = false, bool includeCreated = false, bool includeTipo = false, bool tracking = false)
        {
            var atos = _dbContext.Atos.Where(ato => ato.IsActive);

            if (!tracking)
                atos = atos.AsNoTracking();

            if (includeJurisdicao)
                atos = atos.Include(ato => ato.Jurisdicao);

            if (includeCreated)
                atos = atos.Include(ato => ato.CreatedBy);

            if (includeTipo)
                atos = atos.Include(ato => ato.TipoAto);

            return await atos.FirstOrDefaultAsync(ato => ato.Id.Equals(id));
        }

        public async Task<Ato?> GetByNumber(string number, bool includeJurisdicao = false, bool includeCreated = false, bool includeTipo = false, bool tracking = false)
        {
            var atos = _dbContext.Atos.Where(ato => ato.IsActive);

            if (!tracking)
                atos = atos.AsNoTracking();

            if (includeJurisdicao)
                atos = atos.Include(ato => ato.Jurisdicao);

            if (includeCreated)
                atos = atos.Include(ato => ato.CreatedBy);

            if (includeTipo)
                atos = atos.Include(ato => ato.TipoAto);

            return await atos.FirstOrDefaultAsync(ato => ato.Numero.Replace(".", string.Empty).Equals(number.Replace(".", string.Empty)));
        }

        public async Task<List<Ato>> SearchByNumber(string number, bool includeJurisdicao = false, bool includeCreated = false, bool includeTipo = false, bool tracking = false)
        {
            var atos = _dbContext.Atos.Where(ato => ato.IsActive);

            if (!tracking)
                atos = atos.AsNoTracking();

            if (includeJurisdicao)
                atos = atos.Include(ato => ato.Jurisdicao);

            if (includeCreated)
                atos = atos.Include(ato => ato.CreatedBy);

            if (includeTipo)
                atos = atos.Include(ato => ato.TipoAto);

            return await atos.Where(ato => ato.Numero.Replace(".", string.Empty).StartsWith(number.Replace(".", string.Empty))).ToListAsync();
        }

        public async Task<List<Ato>> SearchByTipo(string tipoAto, bool includeJurisdicao = false, bool includeCreated = false, bool tracking = false)
        {
            var atos = _dbContext.Atos.Include(ato => ato.TipoAto).Where(ato => ato.IsActive);

            if (!tracking)
                atos = atos.AsNoTracking();

            if (includeJurisdicao)
                atos = atos.Include(ato => ato.Jurisdicao);

            if (includeCreated)
                atos = atos.Include(ato => ato.CreatedBy);
            
            return await atos.Where(ato => ato.TipoAto.Nome.Equals(tipoAto)).ToListAsync();
        }

        public async Task<List<Ato>> SearchByJurisdicao(string jurisdicao, bool includeCreated = false, bool includeTipo = false, bool tracking = false)
        {
            var atos = _dbContext.Atos.Include(ato => ato.Jurisdicao).Where(ato => ato.IsActive);

            if (!tracking)
                atos = atos.AsNoTracking();

            if (includeCreated)
                atos = atos.Include(ato => ato.CreatedBy);

            if (includeTipo)
                atos = atos.Include(ato => ato.TipoAto);

            return await atos
                .Where(ato => ato.Jurisdicao.Sigla.Equals(jurisdicao)
                || (ato.Jurisdicao.Estado != null && ato.Jurisdicao.Estado.Equals(jurisdicao)))
                .ToListAsync();
        }

        public async Task<List<Ato>> SearchByJurisdicao(EAmbitoType ambito, bool includeCreated = false, bool includeTipo = false, bool tracking = false)
        {
            var atos = _dbContext.Atos.Include(ato => ato.Jurisdicao).Where(ato => ato.IsActive);

            if (!tracking)
                atos = atos.AsNoTracking();

            if (includeCreated)
                atos = atos.Include(ato => ato.CreatedBy);

            if (includeTipo)
                atos = atos.Include(ato => ato.TipoAto);

            return await atos.Where(ato => ato.Jurisdicao.Ambito.Equals(ambito)).ToListAsync();
        }

        public async Task<bool> Exists(Ato ato)
        {
            return await _dbContext.Atos
                .CountAsync(atoEntity => 
                    atoEntity.TipoAtoId.Equals(ato.TipoAtoId)
                    && atoEntity.Numero.Replace(".", string.Empty).Equals(ato.Numero.Replace(".", string.Empty))
                    && atoEntity.JurisdicaoId.Equals(ato.JurisdicaoId)
                    && atoEntity.IsActive) > 0;
        }
    }
}
