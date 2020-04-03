using ATM.MQ.Core.Entities;

namespace ATM.MQ.Core.Interfaces.Repositories
{
	public interface IMessageRepository : IGenericRepository<MessageData<Transaction>>
	{
	}
}
