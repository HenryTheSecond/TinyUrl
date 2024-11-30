using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Interfaces;
using System.Linq.Expressions;

namespace Shared.Repositories
{
    public abstract class BaseMongoDbRepository<TDocument>(MongoClient mongoClient, string database, string collectionName) 
        : IMongoDbRepository<TDocument> where TDocument : IMongoDbDocument
    {
        protected IMongoCollection<TDocument> collection = mongoClient
            .GetDatabase(database)
            .GetCollection<TDocument>(collectionName);
        
        public async Task DeleteAsync(ObjectId id, IClientSessionHandle? session = null)
        {
            if (session == null)
            {
                await collection.DeleteOneAsync(x => x.Id == id);
                return;
            }
            await collection.DeleteOneAsync(session, x => x.Id == id); 
        }

        public async Task<List<TDocument>> FindAsync(Expression<Func<TDocument, bool>>? predicate = null, IClientSessionHandle? session = null)
        {
            var filter = GetFilterDefinition(predicate);
            if (session == null)
            {
                return await collection.Find(filter).ToListAsync();
            }
            return await collection.Find(session, filter).ToListAsync();
        }

        public async Task<TDocument?> FindOneAsync(Expression<Func<TDocument, bool>>? predicate = null, IClientSessionHandle? session = null)
        {
            var filter = GetFilterDefinition(predicate);
            if (session == null)
            {
                return await collection.Find(filter).FirstOrDefaultAsync();
            }

            return await collection.Find(session, filter).FirstOrDefaultAsync();
        }

        public async Task InsertAsync(TDocument document, IClientSessionHandle? session = null)
        {
            if (session == null)
            {
                await collection.InsertOneAsync(document);
                return;
            }
            await collection.InsertOneAsync(session, document);
        }

        public async Task ReplaceAsync(TDocument document, IClientSessionHandle? session = null)
        {
            if(session == null)
            {
                await collection.ReplaceOneAsync(x => x.Id == document.Id, document);
                return;
            }
            await collection.ReplaceOneAsync(session, x => x.Id == document.Id, document);
        }

        protected static FilterDefinition<TDocument> GetFilterDefinition(Expression<Func<TDocument, bool>>? predicate)
        {
            return predicate == null ? Builders<TDocument>.Filter.Empty :
                Builders<TDocument>.Filter.Where(predicate);
        }
    }
}
