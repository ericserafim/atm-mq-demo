using ATM.MQ.Core.Entities;
using System;
using System.Threading.Tasks;

namespace ATM.MQ.Core.Interfaces.Services
{
	public interface IMessageService : IDisposable
	{
		Task SendMessageAsync(string senderId, MessageData<Transaction> message);

		Task SubscribeQueueAsync(string queueName);
	}
}
