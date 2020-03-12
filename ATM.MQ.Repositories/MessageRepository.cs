using System;
using System.Threading.Tasks;
using ATM.MQ.Core.Entities;
using ATM.MQ.Core.Interfaces.Repositories;

namespace ATM.MQ.Repositories
{
    public class MessageRepository : IMessageRepository<MessageData<Transaction>>
    {
        public MessageRepository()
        {
        }

        public Task<bool> DeleteMessageAsync(long Id)
        {
            throw new NotImplementedException();
        }

        public Task<MessageData<Transaction>> GetMessageAsync(long Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveMessageAsync(MessageData<Transaction> message)
        {
            throw new NotImplementedException();
        }
    }
}
