using System.Threading.Tasks;
using ATM.MQ.Core.Entities;

namespace ATM.MQ.Core.Interfaces.Services
{
    public interface IMessageService<T> where T : class
    {        
        Task<bool> SendMessageAsync(string queueName, MessageData<Transaction> message);
        
        Task SubscribeQueueAsync(string queueName);
    }
}
