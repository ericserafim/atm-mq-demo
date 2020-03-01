using System.Threading.Tasks;
using ATM.MQ.Core.Interfaces.Repositories;

namespace ATM.MQ.Core.Interfaces.Services
{
    public interface IMessageService<T> where T : class
    {
        public IMessageRepository<T> Repository { get; }

        Task<bool> DeleteMessageAsync(long id);
        
        Task<T> GetMessageAsync(long id);

        Task<bool> SaveMessageAsync(T message);
    }
}
