using System;
using Microsoft.Extensions.DependencyInjection;
using ATM.MQ.Application.Services;
using ATM.MQ.Core.Interfaces;

namespace ATM.MQ.Backend
{
    class Program
    {
        static void Main(string[] args)
        {
            var collection = new ServiceCollection();
            collection.AddScoped<IMessageService, MessageService>();
            
            var serviceProvider = collection.BuildServiceProvider();            
        }
    }
}
