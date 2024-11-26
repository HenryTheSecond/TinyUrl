﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Interfaces;

namespace TinyUrlJobs.Models;

public class TinyUrl : IMongoDbDocument
{
    public ObjectId Id { get; set; }
    public string ShortUrl { get; set; } = string.Empty;
    public string OriginalUrl { get; set; } = string.Empty;
    [BsonRepresentation(BsonType.DateTime)]
    public DateTimeOffset Expire { get; set; }
    public UserInfo? UserInfo { get; set; }
}
