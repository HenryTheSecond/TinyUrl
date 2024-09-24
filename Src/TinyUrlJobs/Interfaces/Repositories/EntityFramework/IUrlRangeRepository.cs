using Shared.Interfaces;
using Shared.Models.Database;

namespace TinyUrlJobs.Interfaces.Repositories.EntityFramework
{
    public interface IUrlRangeRepository : IEntityFrameworkRepository<UrlRange>
    {
        Task<long> CountRemaining();
    }
}
