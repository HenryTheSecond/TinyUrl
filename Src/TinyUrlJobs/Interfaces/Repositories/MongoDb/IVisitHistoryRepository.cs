using Shared.Interfaces;
using TinyUrlJobs.Models;

namespace TinyUrlJobs.Interfaces.Repositories.MongoDb;

public interface IVisitHistoryRepository : IMongoDbRepository<VisitHistory>
{
    Task<List<(string TinyUrl, long Count, DateTimeOffset MaxVisitedTime)>> GetUnaggregatedVisitedTimesByUrl(
        IEnumerable<(string TinyUrl, DateTimeOffset LastVisitedTimeAggregate)> tinyUrls);
}
