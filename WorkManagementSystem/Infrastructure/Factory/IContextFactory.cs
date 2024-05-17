using WorkManagementSystem.Infrastructure.Persistence;

namespace WorkManagementSystem.Infrastructure.Factory
{
    public interface IContextFactory
    {
        IDatabaseContext DbContext { get; }
    }
}
