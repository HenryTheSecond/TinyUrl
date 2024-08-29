using Microsoft.EntityFrameworkCore;
using Shared.Models.Database;
using Shared.Repositories;
using WriteTinyUrl.Interfaces.Repositories;

namespace WriteTinyUrl.Repositories
{
    public class UrlRangeRepository(UrlRangeContext context) : BaseRepository<UrlRange>(context), IUrlRangeRepository
    {
        public async Task<UrlRange?> GetTinyUrlAsync()
        {
            return await Query.FirstOrDefaultAsync(x => x.IsUsed == false);
        }
    }
}
