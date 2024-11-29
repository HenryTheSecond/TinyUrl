using MongoDB.Driver;
using Shared.Attributes;
using Shared.Repositories;
using WriteTinyUrl.Interfaces.Repositories.MongoDb;
using WriteTinyUrl.Models;

namespace WriteTinyUrl.Repositories.MongoDb;

[Export(LifeCycle = LifeCycle.SINGLETON)]
public class VisitHistoryRepository(MongoClient mongoClient) :
    BaseMongoDbRepository<VisitHistory>(mongoClient, "TinyUrlDb", "VisitHistory"),
    IVisitHistoryRepository
{
    public async Task<List<(string TinyUrl, long VisitedTimes)>> GetUnaggregatedVisitedTimesByUrl(IEnumerable<(string TinyUrl, DateTimeOffset LastVisitedTimeAggregate)> tinyUrls)
    {
        /*var result = collection.AsQueryable()
            .Where(x => tinyUrls.Any(url => url.TinyUrl == x.TinyUrl && url.LastVisitedTimeAggregate < x.VisitedTime))
            .GroupBy(x => x.TinyUrl)
            .Select(x => new { TinyUrl = x.Key, VisitedTimes = x.LongCount() })
            .ToList()
            .Select(x => (x.TinyUrl, x.VisitedTimes))
            .ToList();
        return Task.FromResult(result);*/

        var filter = Builders<VisitHistory>.Filter
            .Where(x => tinyUrls.Any(url => url.TinyUrl == x.TinyUrl && url.LastVisitedTimeAggregate < x.VisitedTime));

        return (await collection
            .Aggregate()
            .Match(filter)
            .Group(x => x.TinyUrl, x => new { TinyUrl = x.Key, VisitedTimes = x.LongCount() })
            .ToListAsync())
            .Select(x => (x.TinyUrl, x.VisitedTimes))
            .ToList();
    }
}