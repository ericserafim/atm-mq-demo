using ATM.MQ.Application.Exceptions;
using ATM.MQ.Core.Entities;
using ATM.MQ.Core.Interfaces.MQ;
using ATM.MQ.Core.Interfaces.Repositories;
using ATM.MQ.Core.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace ATM.MQ.Application.Services
{
	public class MessageService : IMessageService
	{
		private readonly IMQProvider _mqProvider;

		private readonly IContextRepository _contextRepository;


		public MessageService(IMQProviderFactory providerFactory, IContextRepository contextRepository)
		{
			_mqProvider = providerFactory.Create();
			_contextRepository = contextRepository;
			_mqProvider.OnReceivedMessage += OnReceivedMessage;
			_mqProvider.Connect();
		}

		private void OnReceivedMessage(object sender, MessageData<Transaction> message)
		{
			_contextRepository.Messages.InsertAsync(message);
		}

		public async Task SendMessageAsync(string senderId, MessageData<Transaction> message)
		{
			if (message is null)
				throw new InvalidMessageDataException("Message should not be null");

			await Task.Factory.StartNew(() => _mqProvider.PublishMessage(senderId, message));
		}

		public async Task SubscribeQueueAsync(string queueName)
		{
			if (string.IsNullOrWhiteSpace(queueName))
				throw new ArgumentNullException();

			await Task.Factory.StartNew(() => _mqProvider.SubscribeQueue(queueName));
		}

		public void Dispose()
		{
			this._mqProvider?.Close();
		}
	}
}
