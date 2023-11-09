using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Storage;
using PortalLegisAmbiental.Domain.Seedwork;
using PortalLegisAmbiental.Domain.Entities;
using PortalLegisAmbiental.Domain.Enums;
using System.Data;

namespace PortalLegisAmbiental.Infrastructure
{
    public sealed class EfDbContext : DbContext, IUnitOfWork
    {
        public EfDbContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Environment.GetEnvironmentVariable("TCC_CONNECTION_STRING");
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

        public DbSet<Ato> Atos { get; set; }
        public DbSet<ConteudoAto> ConteudosAtos { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<Jurisdicao> Jurisdicoes { get; set; }
        public DbSet<Permissao> Permissoes { get; set; }
        public DbSet<TipoAto> TiposAtos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Enum converters
            var converterAmbitoType = new EnumToStringConverter<EAmbitoType>();
            var converterScopeType = new EnumToStringConverter<EScopeType>();

            modelBuilder
                .Entity<Jurisdicao>()
                .Property(e => e.Ambito)
                .HasConversion(converterAmbitoType);

            modelBuilder
                .Entity<Permissao>()
                .Property(e => e.Scope)
                .HasConversion(converterScopeType);

            // One-to-One relations
            modelBuilder
                .Entity<Ato>()
                .HasOne(a => a.Jurisdicao)
                .WithOne()
                .HasForeignKey<Ato>(a => a.JurisdicaoId)
                .IsRequired();

            modelBuilder
                .Entity<Ato>()
                .HasOne(a => a.ConteudoAto)
                .WithOne()
                .HasForeignKey<ConteudoAto>(a => a.AtoId)
                .IsRequired();

            modelBuilder
                .Entity<Ato>()
                .HasOne(a => a.CreatedBy)
                .WithOne()
                .HasForeignKey<Ato>(a => a.CreatedById)
                .IsRequired();

            modelBuilder
                .Entity<Ato>()
                .HasOne(a => a.TipoAto)
                .WithOne()
                .HasForeignKey<Ato>(a => a.TipoAtoId)
                .IsRequired();

            // Many-to-Many relations
            modelBuilder
                .Entity<Usuario>()
                .HasMany(u => u.Grupos)
                .WithMany(g => g.Usuarios)
                .UsingEntity<UsuarioGrupo>();

            modelBuilder
                .Entity<Grupo>()
                .HasMany(g => g.Permissoes)
                .WithMany(p => p.Grupos)
                .UsingEntity<GrupoPermissao>();
        }

        public IDbContextTransaction _currentTransaction => Database.CurrentTransaction;

        private DbTransaction _transaction;

        public IEfDbTransaction BeginOrGetCurrentTransaction()
        {
            if (_transaction != null)
            {
                if (_transaction.GetCurrentTransaction().GetDbTransaction().Connection?.State != ConnectionState.Open)
                {
                    _transaction.GetCurrentTransaction().GetDbTransaction().Connection?.Open();
                }

                return _transaction;
            }

            _transaction = new DbTransaction(this);
            return _transaction;
        }

        public IEfDbTransaction BeginTransaction()
        {
            _transaction = new DbTransaction(this);
            return _transaction;
        }

        public async Task TransactionCommitAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_transaction != null)
                {
                    var current = _transaction.GetCurrentTransaction();
                    await current.CommitAsync(cancellationToken);
                }
            }
            catch (Exception)
            {
                Console.Error.WriteLine();
            }
        }

        public async Task TransactionRollbackAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_transaction != null)
                {
                    var current = _transaction.GetCurrentTransaction();
                    await current.RollbackAsync(cancellationToken);
                }
            }
            catch (Exception)
            {
                Console.Error.WriteLine();
            }
        }
    }
}