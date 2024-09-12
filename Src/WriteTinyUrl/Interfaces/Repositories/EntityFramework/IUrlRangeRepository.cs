using Shared.Interfaces;
using Shared.Models.Database;

namespace WriteTinyUrl.Interfaces.Repositories.EntityFramework
{
    public interface IUrlRangeRepository : IEntityFrameworkRepository<UrlRange>
    {
        Task<UrlRange?> GetOneKeyRangeAsync();
    }
}
