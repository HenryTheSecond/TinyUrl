using Shared.Interfaces;
using WriteTinyUrl.Models;

namespace WriteTinyUrl.Interfaces.Repositories.MongoDb;

public interface IVisitHistoryRepository : IMongoDbRepository<VisitHistory>
{
    Task<List<(string TinyUrl, long VisitedTimes)>> GetUnaggregatedVisitedTimesByUrl(IEnumerable<(string TinyUrl, DateTimeOffset LastVisitedTimeAggregate)> tinyUrls);
}
