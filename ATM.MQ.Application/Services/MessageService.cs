using ATM.MQ.Application.Exceptions;
using ATM.MQ.Core.Entities;
using ATM.MQ.Core.Entities.DTO;
using ATM.MQ.Core.Interfaces.Configuration;
using ATM.MQ.Core.Interfaces.MQ;
using ATM.MQ.Core.Interfaces.Repositories;
using ATM.MQ.Core.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace ATM.MQ.Application.Services
{
	public class MessageService : IMessageService
	{
		private readonly IMQProvider<MessageData<Transaction>> _mqProvider;

		private readonly IContextRepository _contextRepository;

    private readonly IAppSettings _appSettings;

    public MessageService(IMQProvider<MessageData<Transaction>> mqProvider, IContextRepository contextRepository, IAppSettings appSettings)
		{
			_mqProvider = mqProvider;
			_contextRepository = contextRepository;
      _appSettings = appSettings;
      _mqProvider.OnReceivedMessage += OnReceivedMessage;
			_mqProvider.Connect();
		}

		private void OnReceivedMessage(object sender, MessageData<Transaction> message)
		{
			Console.WriteLine($"Received Message Id {message.Id}");				

			_contextRepository.Messages.InsertAsync(message).GetAwaiter().GetResult();
		}

		public async Task SendMessageAsync(string senderId, MessageData<Transaction> message)
		{
			if (message is null)
				throw new InvalidMessageDataException("Message should not be null");

			await Task.Factory.StartNew(() => _mqProvider.PublishMessage(_appSettings.Exchange, _appSettings.RoutingKey, senderId, message));
		}

		public async Task SubscribeQueueAsync()
		{
			if (string.IsNullOrWhiteSpace(_appSettings.QueueName))
				throw new ArgumentNullException("Queue name cannot be null");

			await Task.Factory.StartNew(() => _mqProvider.SubscribeQueue(_appSettings.QueueName));
		}

		public void Dispose()
		{
			this._mqProvider?.Close();
		}
	}
}
