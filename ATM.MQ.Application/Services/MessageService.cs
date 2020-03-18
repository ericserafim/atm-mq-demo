using ATM.MQ.Application.Exceptions;
using ATM.MQ.Core.Entities;
using ATM.MQ.Core.Interfaces.MQ;
using ATM.MQ.Core.Interfaces.Repositories;
using ATM.MQ.Core.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace ATM.MQ.Application.Services
{
	public class MessageService : IMessageService<MessageData<Transaction>>
	{
		public IMQProvider MQProvider { get; }

		public IMessageRepository<MessageData<Transaction>> Repository { get; }

		public MessageService(IMQProviderFactory providerFactory, IMessageRepository<MessageData<Transaction>> repository)
		{
			this.MQProvider = providerFactory.Create();
			this.Repository = repository;

			MQProvider.ConnectAsync().GetAwaiter().GetResult();
		}

		public async Task<bool> SendMessageAsync(string queueName, MessageData<Transaction> message)
		{
			if (message is null)
				throw new InvalidMessageDataException("Message should not be null");

			await MQProvider.PublishMessageAsync(queueName, message);
			await Repository.SaveMessageAsync(message);

			return true;
		}

		public async Task SubscribeQueueAsync(string queueName)
		{
			if (string.IsNullOrWhiteSpace(queueName))
				throw new ArgumentNullException();

			await MQProvider.SubscribeQueueAsync(queueName);
		}

		public void Dispose()
		{
			this.MQProvider?.Close();
		}
	}
}
