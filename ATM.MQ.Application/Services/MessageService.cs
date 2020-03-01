using System;
using ATM.MQ.Core.Entities;
using ATM.MQ.Core.Interfaces.Services;
using ATM.MQ.Core.Interfaces.Repositories;
using System.Threading.Tasks;
using ATM.MQ.Application.Exceptions;

namespace ATM.MQ.Application.Services
{
    public class MessageService : IMessageService<MessageData<Transaction>>
    {
        public IMessageRepository<MessageData<Transaction>> Repository { get; }

        public MessageService(IMessageRepository<MessageData<Transaction>> repository)
        {
            this.Repository = repository;
        }

        public async Task<bool> SaveMessageAsync(MessageData<Transaction> message)
        {
            return await Repository.SaveMessageAsync(message ?? throw new InvalidMessageDataException("Message should not be null"));
        }

        public async Task<MessageData<Transaction>> GetMessageAsync(long id)
        {
            try
            {
                return await Repository.GetMessageAsync(id);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteMessageAsync(long id)
        {
            _ = await Repository.GetMessageAsync(id) ??
            throw new DataNotFoundException($"Message with Id {id} was not found");

            return await Repository.DeleteMessageAsync(id);            
        }
    }
}
