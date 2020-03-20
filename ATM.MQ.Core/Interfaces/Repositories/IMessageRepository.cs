using System.Threading.Tasks;

namespace ATM.MQ.Core.Interfaces.Repositories
{
	public interface IMessageRepository<T> where T : class
	{
		Task<T> GetMessageAsync(string Id);

		Task<bool> SaveMessageAsync(T message);

		Task<bool> DeleteMessageAsync(string Id);
    }
}
