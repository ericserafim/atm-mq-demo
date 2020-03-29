using Microsoft.Extensions.DependencyInjection;
using ATM.MQ.Backend.Extensions;
using ATM.MQ.Core.Interfaces.Services;
using ATM.MQ.Core.Interfaces.Configuration;

namespace ATM.MQ.Backend
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.RegisterServices();
            
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var messageService = serviceProvider.GetService<IMessageService>();
            var mqSettings = serviceProvider.GetService<IMQConnectionSettings>();

            messageService.SubscribeQueueAsync(mqSettings.QueueName);                                
        }
    }
}
