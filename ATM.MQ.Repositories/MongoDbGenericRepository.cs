using ATM.MQ.Application.Exceptions;
using ATM.MQ.Core.Entities;
using ATM.MQ.Core.Interfaces.Repositories;
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

		public async Task DeleteAsync(object id)
		{
			var result = await _collection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("Id", id));

			if (result.DeletedCount != 1)
				throw new DataNotFoundException($"Object with Id {id} not found to delete");
		}

		public async Task<TEntity> GetAsync(object id)
		{
			var result = _collection.Find(Builders<TEntity>.Filter.Eq("Id", id));
			return await result.SingleOrDefaultAsync();
		}

		public async Task InsertAsync(TEntity entity)
		{
			await _collection.InsertOneAsync(entity);
		}

		public async Task UpdateAsync(TEntity entity)
		{
			var result = await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = false });

			if (!result.IsModifiedCountAvailable)
				throw new DataNotFoundException($"Object with Id {entity.Id} not found to update");
		}
	}
}
