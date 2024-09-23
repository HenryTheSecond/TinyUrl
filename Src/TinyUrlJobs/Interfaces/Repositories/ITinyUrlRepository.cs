using MongoDB.Driver;
using Shared.Interfaces;
using Shared.Models.MongoDb;
using System.Linq.Expressions;

namespace TinyUrlJobs.Interfaces.Repositories
{
    public interface ITinyUrlRepository : IMongoDbRepository<TinyUrl>
    {
        Task<List<TinyUrl>> FindBatchAsync(Expression<Func<TinyUrl, bool>> predicate, int? batchSize, IClientSessionHandle? session = null);
    }
}
