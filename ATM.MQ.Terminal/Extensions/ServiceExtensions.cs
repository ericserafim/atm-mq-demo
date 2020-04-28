using ATM.MQ.Application.Services;
using ATM.MQ.Core.Entities;
using ATM.MQ.Core.Interfaces.Configuration;
using ATM.MQ.Core.Interfaces.MQ;
using ATM.MQ.Core.Interfaces.Repositories;
using ATM.MQ.Core.Interfaces.Services;
using ATM.MQ.RabbitMQ;
using ATM.MQ.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.IO;

namespace ATM.MQ.Terminal.Extensions
{
	public static class ServiceExtensions
	{
		public static void RegisterServices(this IServiceCollection serviceCollection)
		{
			var configuration = GetConfiguration();
			
			serviceCollection.AddScoped<IMQProvider<MessageData<Transaction>>, RabbitMQProvider<MessageData<Transaction>>>();
			serviceCollection.AddScoped<IContextRepository, MongoDbContext>();
			serviceCollection.AddScoped<IMessageService, MessageService>();			

			serviceCollection.Configure<DatabaseSettings>(configuration.GetSection(nameof(DatabaseSettings)));
			serviceCollection.AddSingleton<IDatabaseSettings>(sp => sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

			serviceCollection.Configure<MQConnectionSettings>(configuration.GetSection(nameof(MQConnectionSettings)));
			serviceCollection.AddSingleton<IMQConnectionSettings>(sp => sp.GetRequiredService<IOptions<MQConnectionSettings>>().Value);

			serviceCollection.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));
			serviceCollection.AddSingleton<IAppSettings>(sp => sp.GetRequiredService<IOptions<AppSettings>>().Value);			
		}

		private static IConfiguration GetConfiguration() =>
				new ConfigurationBuilder()
						.SetBasePath(Directory.GetCurrentDirectory())
						.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
						.Build();
	}
}
