using MongoDB.Driver;
using Shared.Attributes;
using Shared.Models.MongoDb;
using Shared.Repositories;
using System.Linq.Expressions;
using TinyUrlJobs.Interfaces.Repositories;

namespace TinyUrlJobs.Repositories
{
    [Export(LifeCycle = LifeCycle.SINGLETON)]
    public class TinyUrlRepository(MongoClient mongoClient) :
        BaseMongoDbRepository<TinyUrl>(mongoClient, "TinyUrlDb", "TinyUrl"),
        ITinyUrlRepository
    {
        public async Task<List<TinyUrl>> FindBatchAsync(Expression<Func<TinyUrl, bool>> predicate, int? batchSize, IClientSessionHandle? session = null)
        {
            var query = session == null ? collection.Find(predicate, new FindOptions { BatchSize = batchSize }) :
                collection.Find(session, predicate, new FindOptions { BatchSize = batchSize });

            return await query.SortBy(x => x.Id).ToListAsync();
        }
    }
}
