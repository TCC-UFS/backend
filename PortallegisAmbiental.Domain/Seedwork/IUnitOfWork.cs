namespace PortalLegisAmbiental.Domain.Seedwork
{
    public interface IUnitOfWork
    {
        IEfDbTransaction BeginTransaction();
        IEfDbTransaction BeginOrGetCurrentTransaction();
        Task TransactionCommitAsync(CancellationToken cancellationToken = default);
        Task TransactionRollbackAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
    }
}
