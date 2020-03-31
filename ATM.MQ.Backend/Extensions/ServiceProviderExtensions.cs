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

namespace ATM.MQ.Backend.Extensions
{
  public static class ServiceProviderExtensions
  {
    public static void RegisterServices(this ServiceCollection serviceCollection)
    {
      var configuration = GetConfiguration();

      serviceCollection.AddScoped<IMQProviderFactory, RabbitMQProviderFactory>();
      serviceCollection.AddScoped<IMQProvider, RabbitMQProvider>();
      serviceCollection.AddScoped<IMessageRepository, MongoDbMessageRepository>();
      serviceCollection.AddScoped<IMessageService, MessageService>();

      serviceCollection.Configure<DatabaseSettings>(configuration.GetSection(nameof(DatabaseSettings)));
      serviceCollection.AddSingleton<IDatabaseSettings>(sp => sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

      serviceCollection.Configure<MQConnectionSettings>(configuration.GetSection(nameof(MQConnectionSettings)));
      serviceCollection.AddSingleton<IMQConnectionSettings>(sp => sp.GetRequiredService<IOptions<MQConnectionSettings>>().Value);
    }

    private static IConfiguration GetConfiguration() =>
        new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
  }
}
