using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace ATM.MQ.Backend
{
  class Program
	{
		static void Main(string[] args)
		{
			AppStart.Initialize();
		
			var logger = CreateLogger();
			logger.LogInformation("Backend is running");
			
			var messageService = AppStart.GetMessageService();	
			try
			{
				messageService.SubscribeQueueAsync();
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
