using MongoDB.Driver;
using ReadTinyUrl.Interfaces.Repositories;
using Shared.Attributes;
using Shared.Models.MongoDb;
using Shared.Repositories;

namespace ReadTinyUrl.Repositories
{
    [Export(LifeCycle = LifeCycle.SINGLETON)]
    public class TinyUrlRepository(MongoClient mongoClient) : 
        BaseMongoDbRepository<TinyUrl>(mongoClient, "TinyUrlDb", "TinyUrl"), ITinyUrlRepository
    {
    }
}
