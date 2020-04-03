using ATM.MQ.Core.Entities;

namespace ATM.MQ.Core.Interfaces.Repositories
{
	public interface IContextRepository
	{
		IGenericRepository<Account> Accounts { get; }

		IGenericRepository<Owner> Owners { get; }

		IGenericRepository<MessageData<Transaction>> Messages { get; }

		void SaveChanges();
	}
}
