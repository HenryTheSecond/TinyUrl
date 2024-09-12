﻿using MongoDB.Bson;
using Shared.Interfaces;

namespace Shared.Models.MongoDb
{
    public class TinyUrl : IMongoDbDocument
    {
        public ObjectId Id { get; set; }
        public string ShortUrl { get; set; } = string.Empty;
        public string OriginalUrl { get; set; } = string.Empty;
        public DateTimeOffset Expire { get; set; }
    }
}