using Microsoft.EntityFrameworkCore.Storage;
using PortalLegisAmbiental.Domain.Seedwork;

namespace PortalLegisAmbiental.Infrastructure
{
    public sealed class DbTransaction : IEfDbTransaction
    {
        private readonly IDbContextTransaction _transaction;

        public DbTransaction(EfDbContext context)
        {
            if (context.Database.CurrentTransaction is null)
            {
                _transaction = context.Database.BeginTransaction();
            }
            else
            {
                _transaction = context.Database.CurrentTransaction;
            }
        }

        public IDbContextTransaction GetCurrentTransaction() => _transaction;

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return _transaction.CommitAsync(cancellationToken);
        }

        public Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            return _transaction.RollbackAsync(cancellationToken);
        }
    }
}