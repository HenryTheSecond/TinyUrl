using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Interfaces;

namespace UserGetUrlConsumer.Models;

public class VisitHistory : IMongoDbDocument
{
    public ObjectId Id { get; set; }
    public string? UserId { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string TinyUrl { get; set; } = string.Empty;
    public string OriginalUrl { get; set; } = string.Empty;

    [BsonRepresentation(BsonType.DateTime)]
    public DateTimeOffset VisitedTime = DateTimeOffset.UtcNow;
}
