using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Interfaces;

namespace TinyUrlJobs.Models;

public class TinyUrl(UserInfo user, string shortUrl, string originalUrl) : IMongoDbDocument
{
    public ObjectId Id { get; set; }
    public string ShortUrl { get; set; } = shortUrl;
    public string OriginalUrl { get; set; } = originalUrl;

    [BsonRepresentation(BsonType.DateTime)]
    public DateTimeOffset Expire { get; set; }
    public UserInfo UserInfo { get; set; } = user;

    [BsonRepresentation(BsonType.DateTime)]
    public DateTimeOffset CreatedDateTime { get; set; } = DateTimeOffset.UtcNow;
    public long VisitedTime { get; set; } = 0;

    [BsonRepresentation(BsonType.DateTime)]
    public DateTimeOffset LastVisitedTimeAggregate { get; set; } = DateTimeOffset.UtcNow;
}

