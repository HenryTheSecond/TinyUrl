using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Attributes;
using Shared.Interfaces;
using WriteTinyUrl.Interfaces.Repositories.EntityFramework;
using WriteTinyUrl.Interfaces.Repositories.MongoDb;
using WriteTinyUrl.Interfaces.Services;
using WriteTinyUrl.Models;

namespace WriteTinyUrl.Services
{
    [Export(LifeCycle = LifeCycle.SCOPE)]
    public class TinyUrlService(IUrlRangeRepository urlRangeRepository, ITransactionContext transactionContext,
        MongoClient mongoClient, ITinyUrlRepository tinyUrlRepository) : ITinyUrlService
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

            await tinyUrlRepository.InsertAsync(new()
            {
                Id = new ObjectId(),
                ShortUrl = keyRangeUrl.Url,
                Expire = DateTime.UtcNow.AddYears(1),
                OriginalUrl = originalUrl,
                UserInfo = userInfo
            }, mongoDbSession);

            await transactionContext.CommitTransactionAsync();
            await mongoDbSession.CommitTransactionAsync();

            return keyRangeUrl.Url;
        }
    }
}
