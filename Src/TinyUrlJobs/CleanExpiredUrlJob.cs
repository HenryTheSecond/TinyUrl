using MongoDB.Bson;
using MongoDB.Driver;
using Quartz;
using TinyUrlJobs.Interfaces.Repositories;

namespace TinyUrlJobs
{
    [DisallowConcurrentExecution]
    public class CleanExpiredUrlJob(MongoClient mongoClient, ITinyUrlRepository tinyUrlRepository) : IJob
    {
        public const int BatchSize = 100;
        public async Task Execute(IJobExecutionContext context)
        {
            var now = DateTimeOffset.Now;
            ObjectId? idOffset = null;

            using var session = await mongoClient.StartSessionAsync();

            while (true)
            {
                session.StartTransaction();

                // We need to filter with the id offset of every batch we receive due to read and write concerns of MongoDb.
                // We might read the old records if the write is not done yet in replicas.
                // It could also result in better performance if we avoid using majority concern and the id in MongoDb is always indexed.
                var expireUrls = await tinyUrlRepository.FindBatchAsync(x => x.Expire <= now && (idOffset == null || x.Id > idOffset), 
                    BatchSize, session);

                if (expireUrls.Count == 0)
                {
                    break;
                }

                idOffset = expireUrls.Last().Id;

                foreach (var url in expireUrls)
                {
                    await tinyUrlRepository.DeleteAsync(url.Id, session);
                }

                await session.CommitTransactionAsync();
            }
        }
    }
}
