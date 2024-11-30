using MongoDB.Driver;
using Shared.Attributes;
using Shared.Repositories;
using TinyUrlJobs.Interfaces.Repositories.MongoDb;
using TinyUrlJobs.Models;

namespace TinyUrlJobs.Repositories.MongoDb;

[Export(LifeCycle = LifeCycle.SINGLETON)]
public class VisitHistoryRepository(MongoClient mongoClient) : 
    BaseMongoDbRepository<VisitHistory>(mongoClient, "TinyUrlDb", "VisitHistory"), 
    IVisitHistoryRepository
{
    public async Task<List<(string TinyUrl, long Count, DateTimeOffset MaxVisitedTime)>> GetUnaggregatedVisitedTimesByUrl(IEnumerable<(string TinyUrl, DateTimeOffset LastVisitedTimeAggregate)> tinyUrls)
    {
        var filterBuilder = Builders<VisitHistory>.Filter;
        var filter = filterBuilder
            .Or(tinyUrls.Select(url => filterBuilder.Eq(x => x.TinyUrl, url.TinyUrl) & filterBuilder.Gt(x => x.VisitedTime, url.LastVisitedTimeAggregate)));
        
        return (await collection
            .Aggregate()
            .Match(filter)
            .Group(x => x.TinyUrl, x => new { TinyUrl = x.Key, Count = x.LongCount(), MaxVisitedTime = x.Max(x => x.VisitedTime)})
            .ToListAsync())
            .Select(x => (x.TinyUrl, x.Count, x.MaxVisitedTime))
            .ToList();
    }
}
