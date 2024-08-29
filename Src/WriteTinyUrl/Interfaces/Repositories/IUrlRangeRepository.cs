using Shared.Interfaces;
using Shared.Models.Database;

namespace WriteTinyUrl.Interfaces.Repositories
{
    public interface IUrlRangeRepository : IRepository<UrlRange>
    {
        Task<UrlRange?> GetTinyUrlAsync();
    }
}
