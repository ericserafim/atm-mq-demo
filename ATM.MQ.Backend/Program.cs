using ATM.MQ.Backend.Extensions;
using ATM.MQ.Core.Interfaces.Configuration;
using ATM.MQ.Core.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

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

			var logger = CreateLogger();
			logger.LogInformation("Backend is running");

			try
			{
				messageService.SubscribeQueueAsync(mqSettings.QueueName);
				Console.ReadLine();
			}
			finally
			{
				messageService.Dispose();
				logger.LogInformation("Backend stopped");
			}
		}

		private static ILogger<Program> CreateLogger()
		{
			var loggerFactory = LoggerFactory.Create(builder =>
			{
				builder
									.AddFilter("Microsoft", LogLevel.Warning)
									.AddFilter("System", LogLevel.Warning)
									.AddFilter(Assembly.GetAssembly(typeof(Program)).FullName, LogLevel.Debug)
									.AddConsole();
			});

			return loggerFactory.CreateLogger<Program>();
		}
	}
}
