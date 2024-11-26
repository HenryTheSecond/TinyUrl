using MongoDB.Driver;
using Shared.Interfaces;
using System.Linq.Expressions;
using TinyUrlJobs.Models;

namespace TinyUrlJobs.Interfaces.Repositories.MongoDb
{
    public interface ITinyUrlRepository : IMongoDbRepository<TinyUrl>
    {
        Task<IAsyncCursor<TinyUrl>> FindBatchAsync(Expression<Func<TinyUrl, bool>> predicate, int? batchSize, IClientSessionHandle? session = null);
        Task<long> CountExpireUrl(DateTimeOffset dateTime, IClientSessionHandle? session = null);
    }
}
