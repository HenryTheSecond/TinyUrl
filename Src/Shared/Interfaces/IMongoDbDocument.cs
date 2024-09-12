using MongoDB.Bson;

namespace Shared.Interfaces
{
    public interface IMongoDbDocument
    {
        public ObjectId Id { get; set; }
    }
}
