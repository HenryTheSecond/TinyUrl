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
            IFindFluent<VisitHistory, VisitHistory> query;
            if(lastId == null)
            {
                query = collection.Find(FilterDefinition<VisitHistory>.Empty);
            }
            else
            {
                var lastObjectId = new ObjectId(lastId);
                query = collection
                    .Find(x => x.UserId == userId &&
                    (x.VisitedTime < lastVistedTimeParam || (x.VisitedTime == lastVistedTimeParam && x.Id < lastObjectId)));
            }

            return await query.SortByDescending(x => x.VisitedTime)
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