using System;
using ATM.MQ.Backend.Extensions;
using ATM.MQ.Core.Entities;
using ATM.MQ.Core.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ATM.MQ.Backend
{
  public static class AppStart
  {
    private static ServiceProvider _serviceProvider;

    public static void Initialize()
    {
      var serviceCollection = new ServiceCollection();
      serviceCollection.RegisterServices();
      _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    public static IMessageService GetMessageService() => _serviceProvider.GetService<IMessageService>();
  }
}
