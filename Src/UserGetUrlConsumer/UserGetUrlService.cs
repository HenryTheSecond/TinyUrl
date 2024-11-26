using Confluent.Kafka;
using MongoDB.Bson;
using MongoDB.Driver;
using UserGetUrlConsumer.Interfaces.Repositories;
using UserGetUrlConsumer.Models;

namespace UserGetUrlConsumer;

public class UserGetUrlService(IConsumer<Null, UserVisitMessage> consumer,
    MongoClient mongoClient,
    IVisitHistoryRepository visitHistoryRepository,
    ILogger<UserGetUrlService> logger) : IHostedService
{
    private const string UserGetUrlTopic = "user-get-url";
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("UserGetUrlService starts");
        consumer.Subscribe(UserGetUrlTopic);

        await Task.Run(async () =>
        {

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var message = consumer.Consume(cancellationToken).Message.Value;
                    using var mongoDbSession = await mongoClient.StartSessionAsync();
                    mongoDbSession.StartTransaction();

                    await visitHistoryRepository.InsertAsync(new VisitHistory
                    {
                        Id = new ObjectId(),
                        UserId = message.UserId,
                        Username = message.Username,
                        Email = message.Email,
                        OriginalUrl = message.OriginalUrl ?? string.Empty,
                        TinyUrl = message.TinyUrl
                    });

                    await mongoDbSession.CommitTransactionAsync();
                }
                catch (Exception e)
                {
                    logger.LogCritical(e, "Exception occurs");
                }
            }
        });
    }

public Task StopAsync(CancellationToken cancellationToken)
{
    logger.LogCritical("UserGetUrlService stops");
    return Task.CompletedTask;
}
}
