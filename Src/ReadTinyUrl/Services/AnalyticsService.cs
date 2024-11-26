using Confluent.Kafka;
using ReadTinyUrl.Interfaces.Services;
using ReadTinyUrl.Models;
using Shared.Attributes;
using System.Security.Claims;

namespace ReadTinyUrl.Services;

[Export(LifeCycle = LifeCycle.SINGLETON)]
public class AnalyticsService(IProducer<Null, UserVisitMessage> producer) : IAnalyticsService
{
    private const string UserGetUrlTopic = "user-get-url";
    public async Task SaveAnalyticInfo(ClaimsPrincipal user, string tinyUrl, string? originalUrl)
    {
        var message = new UserVisitMessage { TinyUrl = tinyUrl, OriginalUrl = originalUrl };
        if(user.Identity?.IsAuthenticated == true)
        {
            var claims = user.Claims;
            message.UserId = claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;
            message.Username = claims.Single(x => x.Type == "preferred_username").Value;
            message.Email = claims.SingleOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        }
        
        await producer.ProduceAsync(UserGetUrlTopic, new Message<Null, UserVisitMessage>
        {
            Value = message,
            Timestamp = Timestamp.Default
        });
    }
}
