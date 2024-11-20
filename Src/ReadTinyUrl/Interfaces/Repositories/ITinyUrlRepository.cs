using ReadTinyUrl.Models;
using Shared.Interfaces;

namespace ReadTinyUrl.Interfaces.Repositories
{
    public interface ITinyUrlRepository : IMongoDbRepository<TinyUrl>
    {

    }
}
