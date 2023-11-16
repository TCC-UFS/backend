using PortalLegisAmbiental.Domain.Seedwork;

namespace PortalLegisAmbiental.Domain.IRepositories
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
