using ATM.MQ.Application.Exceptions;
using ATM.MQ.Core.Entities;
using ATM.MQ.Core.Interfaces.Configuration;
using ATM.MQ.Core.Interfaces.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace ATM.MQ.Repositories
{
    public class MongoDbGenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : EntityBase
    {
        private readonly IMongoCollection<TEntity> _collection;

        public MongoDbGenericRepository(IMongoDatabase database, string collectionName)
        {
            _collection = database.GetCollection<TEntity>(collectionName);
        }

        public void Delete(object id)
        {
            var result = _collection.DeleteOne(Builders<TEntity>.Filter.Eq("Id", id));

            if (result.DeletedCount != 1)
                throw new DataNotFoundException($"Object with Id {id} not found to delete");
        }

        public TEntity Get(object id)
        {
            var result = _collection.Find(Builders<TEntity>.Filter.Eq("Id", id));
            return result.SingleOrDefault();
        }

        public void Insert(TEntity entity)
        {
            _collection.InsertOne(entity);
        }

        public void Update(TEntity entity)
        {
            var result = _collection.ReplaceOne(x => x.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = false });

            if (!result.IsModifiedCountAvailable)
                throw new DataNotFoundException($"Object with Id {entity.Id} not found to update");
        }
    }
}
