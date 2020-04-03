using System.Threading.Tasks;

namespace ATM.MQ.Core.Interfaces.Repositories
{
	public interface IGenericRepository<TEntity> where TEntity : class
	{
		Task<TEntity> GetAsync(object id);

		Task InsertAsync(TEntity entity);

		Task UpdateAsync(TEntity entity);

		Task DeleteAsync(object id);
	}
}
