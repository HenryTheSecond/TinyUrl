using MongoDB.Driver;
using Quartz;
using TinyUrlJobs.Interfaces.Repositories.MongoDb;

namespace TinyUrlJobs
{
    [DisallowConcurrentExecution]
    public class CleanExpiredUrlJob(MongoClient mongoClient, ITinyUrlRepository tinyUrlRepository) : IJob
    {
        public const int DefaultBatchSize = 100;
        public const int MaxBatch = 50;
        public async Task Execute(IJobExecutionContext context)
        {
            var now = DateTimeOffset.Now;

            using var session = await mongoClient.StartSessionAsync();

            long countExpire = await tinyUrlRepository.CountExpireUrl(now, session);
            var batchSize = Math.Max(DefaultBatchSize, (int)Math.Ceiling(countExpire * 1.0 / MaxBatch));
            
            using var cursor = await tinyUrlRepository.FindBatchAsync(x => x.Expire <= now, batchSize, session);
            int count = 0;
            await cursor.ForEachAsync(async url =>
            {
                if (count == 0)
                {
                    session.StartTransaction();
                }

                await tinyUrlRepository.DeleteAsync(url.Id, session);
                count++;

                if (count == batchSize)
                {
                    await session.CommitTransactionAsync();
                    count = 0;
                }
            });

            // Remove the redundant expired urls if amount of urls % batch size > 0
            if(count > 0)
            {
                await session.CommitTransactionAsync();
            }
        }
    }
}
