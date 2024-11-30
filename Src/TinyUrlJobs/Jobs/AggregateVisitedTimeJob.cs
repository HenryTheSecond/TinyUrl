using MongoDB.Driver;
using Quartz;
using TinyUrlJobs.Interfaces.Repositories.MongoDb;
using TinyUrlJobs.Models;

namespace TinyUrlJobs.Jobs;

[DisallowConcurrentExecution]
public class AggregateVisitedTimeJob(MongoClient mongoClient, 
    ITinyUrlRepository tinyUrlRepository, IVisitHistoryRepository visitHistoryRepository, 
    ILogger<AggregateVisitedTimeJob> logger) : IJob
{
    public const int DefaultBatchSize = 100;
    public const int MaxBatch = 50;

    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Start job {0} at {1}", nameof(AggregateVisitedTimeJob), DateTimeOffset.UtcNow);

        long count = await tinyUrlRepository.EstimatedTinyUrlCount();

        var batchSize = Math.Max(DefaultBatchSize, (int)Math.Ceiling(count * 1.0 / MaxBatch));
        using var cursor = await tinyUrlRepository.FindBatchAsync(null, batchSize);
        

        while(await cursor.MoveNextAsync() && cursor.Current.Any())
        {
            var session = await mongoClient.StartSessionAsync();
            session.StartTransaction();

            var urls = cursor.Current;
            var updatedInfoByUrl = (await visitHistoryRepository.GetUnaggregatedVisitedTimesByUrl(urls.Select(x => (x.ShortUrl, x.LastVisitedTimeAggregate))))
            .ToDictionary(x => x.TinyUrl);

            await Task.WhenAll(urls.Where(x => updatedInfoByUrl.ContainsKey(x.ShortUrl))
                .Select(x =>
                {
                    var additionVisitedTimes = updatedInfoByUrl[x.ShortUrl].Count;
                    var lastVisitedTimeAggregate = updatedInfoByUrl[x.ShortUrl].MaxVisitedTime;
                    return tinyUrlRepository.AggregateUrlVisitedTimes(x.Id, additionVisitedTimes, lastVisitedTimeAggregate, session);
                }));

            await session.CommitTransactionAsync();
        }

        logger.LogInformation("End job {0} at {1}", nameof(AggregateVisitedTimeJob), DateTimeOffset.UtcNow);
    }
}
