using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Attributes;
using Shared.Repositories;
using System.Linq.Expressions;
using TinyUrlJobs.Interfaces.Repositories.MongoDb;
using TinyUrlJobs.Models;

namespace TinyUrlJobs.Repositories.MongoDb
{
    [Export(LifeCycle = LifeCycle.SINGLETON)]
    public class TinyUrlRepository(MongoClient mongoClient) :
        BaseMongoDbRepository<TinyUrl>(mongoClient, "TinyUrlDb", "TinyUrl"),
        ITinyUrlRepository
    {
        public async Task<IAsyncCursor<TinyUrl>> FindBatchAsync(Expression<Func<TinyUrl, bool>>? predicate, int? batchSize, IClientSessionHandle? session = null)
        {
            var filter = GetFilterDefinition(predicate);
            var query = session == null ? collection.Find(filter, new FindOptions { BatchSize = batchSize }) :
                collection.Find(session, filter, new FindOptions { BatchSize = batchSize });

            return await query.ToCursorAsync();
        }

        public async Task<long> CountExpireUrl(DateTimeOffset dateTime, IClientSessionHandle? session = null)
        {
            return session == null ? await collection.CountDocumentsAsync(x => x.Expire <= dateTime) :
                await collection.CountDocumentsAsync(session, x => x.Expire <= dateTime);
        }

        public async Task<long> EstimatedTinyUrlCount()
        {
            return await collection.EstimatedDocumentCountAsync();
        }

        public async Task AggregateUrlVisitedTimes(ObjectId id, long additionVisitedTimes, 
            DateTimeOffset lastVisitedTimeAggregate, IClientSessionHandle? session = null)
        {
            var update = Builders<TinyUrl>.Update
                .Inc(x => x.VisitedTime, additionVisitedTimes)
                .Set(x => x.LastVisitedTimeAggregate, lastVisitedTimeAggregate);

            if (session != null)
            {
                await collection.UpdateOneAsync(session, x => x.Id == id, update);
            }
            else
            {
                await collection.UpdateOneAsync(x => x.Id == id, update);
            }
        }
    }
}
