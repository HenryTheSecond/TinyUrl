using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Shared.Interfaces;

namespace ReadTinyUrl.Models.MongoModels;

public class VisitHistory : IMongoDbDocument
{
    public ObjectId Id { get; set; }
    public string? UserId { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string TinyUrl { get; set; } = string.Empty;
    public string OriginalUrl { get; set; } = string.Empty;

    [BsonRepresentation(BsonType.DateTime)]
    public DateTimeOffset VisitedTime { get; set; } = DateTimeOffset.UtcNow;
}

