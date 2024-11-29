using Shared.Interfaces;
using WriteTinyUrl.Models;

namespace WriteTinyUrl.Interfaces.Repositories.MongoDb
{
    public interface ITinyUrlRepository : IMongoDbRepository<TinyUrl>
    {
        Task<List<TinyUrl>> GetVisitHistories(string userId, int take, DateTimeOffset? lastCreatedDateTime, string? lastId);
    }
}
