using System;

namespace ATM.MQ.Core.Interfaces.Configuration
{
  public interface IAppSettings
  {
    string Exchange { get; set; }

    string RoutingKey { get; set; }

    string QueueName { get; set; }
  }
}
