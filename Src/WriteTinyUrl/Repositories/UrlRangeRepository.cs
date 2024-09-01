using Microsoft.EntityFrameworkCore;
using Shared.Attributes;
using Shared.Models.Database;
using Shared.Repositories;
using WriteTinyUrl.Interfaces.Repositories;

namespace WriteTinyUrl.Repositories
{
    [Export(LifeCycle = LifeCycle.SCOPE)]
    public class UrlRangeRepository(UrlRangeContext context) : BaseRepository<UrlRange>(context), IUrlRangeRepository
    {
        public async Task<UrlRange?> GetTinyUrlAsync()
        {
            return await context.Set<UrlRange>()
                .FromSqlRaw($"SELECT * FROM {UrlRange.TableName} WITH (XLOCK, READPAST) WHERE {nameof(UrlRange.IsUsed)} = 0")
                .FirstOrDefaultAsync();
        }
    }
}
