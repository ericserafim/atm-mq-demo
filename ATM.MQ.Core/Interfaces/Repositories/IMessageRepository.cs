using ATM.MQ.Core.Entities;
using System.Threading.Tasks;

namespace ATM.MQ.Core.Interfaces.Repositories
{
  public interface IMessageRepository
  {
    Task<MessageData<Transaction>> GetMessageAsync(string Id);

    Task<bool> SaveMessageAsync(MessageData<Transaction> message);

    Task<bool> DeleteMessageAsync(string Id);
  }
}
