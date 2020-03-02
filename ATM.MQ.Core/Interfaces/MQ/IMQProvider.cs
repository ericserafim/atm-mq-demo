using System;
using System.Threading.Tasks;

namespace ATM.MQ.Core.Interfaces.MQ
{
    public interface IMQProvider : IDisposable
    {       
        Task ConnectAsync();

        Task PublishMessageAsync<T>(string queueName, T message);

        Task SubscribeQueueAsync(string queueName);
    }
}
