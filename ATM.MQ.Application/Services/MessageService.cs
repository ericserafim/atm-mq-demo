using System;
using ATM.MQ.Core.Entities;
using ATM.MQ.Core.Interfaces.Services;
using ATM.MQ.Core.Interfaces.Repositories;
using System.Threading.Tasks;
using ATM.MQ.Application.Exceptions;
using ATM.MQ.Core.Interfaces.MQ;

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
        }

        public async Task<bool> SendMessageAsync(string queueName, MessageData<Transaction> message)
        {
            if (message is null)
                throw new InvalidMessageDataException("Message should not be null");

            await MQProvider.ConnectAsync();
            await MQProvider.PublishMessageAsync(queueName, message);
            await Repository.SaveMessageAsync(message);

            return true;
        }

        public async Task SubscribeQueueAsync(string queueName)
        {
            if (string.IsNullOrWhiteSpace(queueName))
                throw new ArgumentNullException();
                
            await MQProvider.ConnectAsync();
            await MQProvider.SubscribeQueueAsync(queueName);
        }
    }
}
