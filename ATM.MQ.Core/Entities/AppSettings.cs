using ATM.MQ.Core.Interfaces.Configuration;

namespace ATM.MQ.Core.Entities
{
  public class AppSettings : IAppSettings
  {
    public string Exchange { get; set; }

    public string RoutingKey { get; set; }

    public string QueueName { get; set; }
  }
}
