using ATM.MQ.Core.Entities;
using System;
using System.Threading.Tasks;

namespace ATM.MQ.Core.Interfaces.Services
{
	public interface IMessageService<T> : IDisposable where T : class
	{
		Task<bool> SendMessageAsync(string queueName, MessageData<Transaction> message);

		Task SubscribeQueueAsync(string queueName);
	}
}
