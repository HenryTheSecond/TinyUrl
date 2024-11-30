using MongoDB.Bson;
using MongoDB.Driver;
using ReadTinyUrl.Interfaces.Repositories;
using ReadTinyUrl.Models.MongoModels;
using ReadTinyUrl.Models.Responses;
using Shared.Attributes;
using Shared.Repositories;

namespace ReadTinyUrl.Repositories
{
    [Export(LifeCycle = LifeCycle.SINGLETON)]
    public class VisitHistoryRepository(MongoClient mongoClient) :
        BaseMongoDbRepository<VisitHistory>(mongoClient, "TinyUrlDb", "VisitHistory"),
        IVisitHistoryRepository
    {
        public async Task<List<VisitHistoryResponse>> GetVisitHistories(string userId, int take, 
            DateTimeOffset? lastVistedTimeParam, string? lastId)
        {
            var filterBuilder = Builders<VisitHistory>.Filter;
            var filter = filterBuilder.Where(x => x.UserId == userId);

            if (lastId != null)
            {
                var lastObjectId = new ObjectId(lastId);
                filter = filter &
                    filterBuilder.Where(x => x.VisitedTime < lastVistedTimeParam || (x.VisitedTime == lastVistedTimeParam && x.Id < lastObjectId));
            }

            return await collection.Find(filter)
                .SortByDescending(x => x.VisitedTime)
                .SortByDescending(x => x.Id)
                .Project(x => new VisitHistoryResponse
                {
                    Id = x.Id.ToString(),
                    OriginalUrl = x.OriginalUrl,
                    TinyUrl = x.TinyUrl,
                    VisitedTime = x.VisitedTime
                })
                .Limit(take).ToListAsync();
        }
    }
}