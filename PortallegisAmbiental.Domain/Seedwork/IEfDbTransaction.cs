namespace PortalLegisAmbiental.Domain.Seedwork
{
    public interface IEfDbTransaction : IDisposable
    {
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
        void Commit();
        void Rollback();
    }
}
