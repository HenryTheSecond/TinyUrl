using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Attributes;
using Shared.Repositories;
using WriteTinyUrl.Interfaces.Repositories.MongoDb;
using WriteTinyUrl.Models;

namespace WriteTinyUrl.Repositories.MongoDb
{
    [Export(LifeCycle = LifeCycle.SINGLETON)]
    public class TinyUrlRepository(MongoClient mongoClient) :
        BaseMongoDbRepository<TinyUrl>(mongoClient, "TinyUrlDb", "TinyUrl"), ITinyUrlRepository
    {
        public async Task<List<TinyUrl>> GetVisitHistories(string userId, int take, 
            DateTimeOffset? lastCreatedDateTime, string? lastId)
        {
            var filterBuilder = Builders<TinyUrl>.Filter;
            var filter = filterBuilder.Where(x => x.UserInfo.Sub == userId);

            if (lastId != null)
            {
                var lastObjectId = new ObjectId(lastId);
                filter = filter & 
                    filterBuilder.Where(x => x.CreatedDateTime < lastCreatedDateTime || (x.CreatedDateTime == lastCreatedDateTime && x.Id < lastObjectId));
            }

            return await collection.Find(filter)
                .SortByDescending(x => x.CreatedDateTime)
                .SortByDescending(x => x.Id)
                .Limit(take).ToListAsync();
        }

    }
}
