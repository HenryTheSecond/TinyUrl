using MongoDB.Driver;
using Shared.Attributes;
using Shared.Models.MongoDb;
using Shared.Repositories;
using WriteTinyUrl.Interfaces.Repositories.MongoDb;

namespace WriteTinyUrl.Repositories.MongoDb
{
    [Export(LifeCycle = LifeCycle.SINGLETON)]
    public class TinyUrlRepository(MongoClient mongoClient) : 
        BaseMongoDbRepository<TinyUrl>(mongoClient, "TinyUrlDb", "TinyUrl"), ITinyUrlRepository
    {
    }
}
