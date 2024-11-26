using MongoDB.Driver;
using Shared.Attributes;
using Shared.Repositories;
using UserGetUrlConsumer.Interfaces.Repositories;
using UserGetUrlConsumer.Models;

namespace UserGetUrlConsumer.Repositories;

[Export(LifeCycle = LifeCycle.SINGLETON)]
public class VisitHistoryRepository(MongoClient mongoClient) : 
    BaseMongoDbRepository<VisitHistory>(mongoClient, "TinyUrlDb", "VisitHistory"), 
    IVisitHistoryRepository
{

}
