using ATM.MQ.Core.Interfaces.Repositories;

namespace ATM.MQ.Core.Interfaces.Services
{
    public interface IMessageService<T>
    {
        public IMessageRepository<T> Repository { get; set; }
    }
}
