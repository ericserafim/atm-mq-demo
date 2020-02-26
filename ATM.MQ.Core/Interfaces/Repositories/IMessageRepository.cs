using ATM.MQ.Core.Entities;

namespace ATM.MQ.Core.Interfaces.Repositories
{
    public interface IMessageRepository<T>
    {
        MessageData<T> GetMessage(long Id);
        
        bool SaveMessage(MessageData<T> messageData);

        bool DeleteMessage(long Id);
    }
}
