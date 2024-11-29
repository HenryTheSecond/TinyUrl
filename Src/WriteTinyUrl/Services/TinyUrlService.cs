using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Attributes;
using Shared.Interfaces;
using WriteTinyUrl.Interfaces.Repositories.EntityFramework;
using WriteTinyUrl.Interfaces.Repositories.MongoDb;
using WriteTinyUrl.Interfaces.Services;
using WriteTinyUrl.Models;
using WriteTinyUrl.Models.Responses;

namespace WriteTinyUrl.Services
{
    [Export(LifeCycle = LifeCycle.SCOPE)]
    public class TinyUrlService(IUrlRangeRepository urlRangeRepository, ITransactionContext transactionContext,
        MongoClient mongoClient, ITinyUrlRepository tinyUrlRepository, IVisitHistoryRepository visitHistoryRepository) : ITinyUrlService
    {
        public async Task<string> CreateTinyUrlAsync(UserInfo userInfo, string originalUrl)
        {
            // Start MongoDb and SQL transaction
            var urlRangeTransactionTask = transactionContext.BeginTransactionAsync();
            var mongoSessionTask = mongoClient.StartSessionAsync();
            Task.WaitAll([urlRangeTransactionTask, mongoSessionTask]);

            using var mongoDbSession = mongoSessionTask.Result;
            mongoDbSession.StartTransaction();

            // TODO: define exception handling
            var keyRangeUrl = await urlRangeRepository.GetOneKeyRangeAsync() ?? throw new Exception("Out of tiny url");

            keyRangeUrl.IsUsed = true;
            // Compulsory to call SaveChangeAsync although we begin a transaction, otherwise the record won't be updated
            await urlRangeRepository.SaveChangeAsync();

            await tinyUrlRepository.InsertAsync(new(userInfo, keyRangeUrl.Url, originalUrl)
            {
                Id = new ObjectId(),
                Expire = DateTime.UtcNow.AddYears(1)
            }, mongoDbSession);

            await transactionContext.CommitTransactionAsync();
            await mongoDbSession.CommitTransactionAsync();

            return keyRangeUrl.Url;
        }

        public async Task<List<CreateUrlHistory>> GetCreateUrlHistories(string userId, int take, DateTimeOffset? lastCreatedDate, string? lastId)
        {
            var createUrlHistories = await tinyUrlRepository.GetVisitHistories(userId, take, lastCreatedDate, lastId);
            
            var visitHistoryDictionary = (await visitHistoryRepository
                .GetUnaggregatedVisitedTimesByUrl(createUrlHistories.Select(x => (x.ShortUrl, x.LastVisitedTimeAggregate))))
                .ToDictionary(x => x.TinyUrl, x => x.VisitedTimes);

            return createUrlHistories.Select(x => new CreateUrlHistory
            {
                Id = x.Id.ToString(),
                CreatedDateTime = x.CreatedDateTime,
                Expire = x.Expire,
                OriginalUrl = x.OriginalUrl,
                ShortUrl = x.ShortUrl,
                VisitedTime = x.VisitedTime + visitHistoryDictionary.GetValueOrDefault(x.ShortUrl)
            }).ToList();
        }
    }
}
