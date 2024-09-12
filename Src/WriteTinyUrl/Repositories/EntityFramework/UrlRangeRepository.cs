using Microsoft.EntityFrameworkCore;
using Shared.Attributes;
using Shared.Models.Database;
using Shared.Repositories;
using WriteTinyUrl.Interfaces.Repositories.EntityFramework;

namespace WriteTinyUrl.Repositories.EntityFramework
{
    [Export(LifeCycle = LifeCycle.SCOPE)]
    public class UrlRangeRepository(UrlRangeContext context) : BaseEntityFrameworkRepository<UrlRange>(context), IUrlRangeRepository
    {
        public async Task<UrlRange?> GetOneKeyRangeAsync()
        {
            return await context.Set<UrlRange>()
                .FromSqlRaw($"SELECT * FROM {UrlRange.TableName} WITH (XLOCK, READPAST) WHERE {nameof(UrlRange.IsUsed)} = 0")
                .FirstOrDefaultAsync();
        }
    }
}
