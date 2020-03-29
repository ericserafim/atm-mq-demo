using System.Threading.Tasks;
using ATM.MQ.Core.Entities;

namespace ATM.MQ.Core.Interfaces.Repositories
{
	public interface IMessageRepository
	{
		Task<MessageData<Transaction>> GetMessageAsync(string Id);

		Task<bool> SaveMessageAsync(MessageData<Transaction> message);

		Task<bool> DeleteMessageAsync(string Id);
    }
}
