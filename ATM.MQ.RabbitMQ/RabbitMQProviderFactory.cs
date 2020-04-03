using ATM.MQ.Core.Interfaces.Configuration;
using ATM.MQ.Core.Interfaces.MQ;

namespace ATM.MQ.RabbitMQ
{
	public class RabbitMQProviderFactory : IMQProviderFactory
	{
		private readonly IMQConnectionSettings _connectionSettings;

		public RabbitMQProviderFactory(IMQConnectionSettings connectionSettings)
		{
			_connectionSettings = connectionSettings;
		}

		public IMQProvider Create() => new RabbitMQProvider(_connectionSettings);
	}
}
