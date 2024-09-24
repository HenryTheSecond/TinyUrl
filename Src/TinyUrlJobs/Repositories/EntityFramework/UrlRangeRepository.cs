using Microsoft.EntityFrameworkCore;
using Shared.Attributes;
using Shared.Models.Database;
using Shared.Repositories;
using TinyUrlJobs.Interfaces.Repositories.EntityFramework;

namespace TinyUrlJobs.Repositories.EntityFramework
{
    [Export(LifeCycle = LifeCycle.SCOPE)]
    public class UrlRangeRepository(UrlRangeContext context) : BaseEntityFrameworkRepository<UrlRange>(context), IUrlRangeRepository
    {
        public Task<long> CountRemaining()
        {
            return Query.LongCountAsync(x => !x.IsUsed);
        }
    }
}
