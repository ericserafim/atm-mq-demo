using ATM.MQ.Core.Interfaces.Configuration;

namespace ATM.MQ.Core.Entities
{
	public class DatabaseSettings : IDatabaseSettings
	{
		public string MessagesCollectionName { get; set; }

		public string ConnectionString { get; set; }

		public string DatabaseName { get; set; }
	}
}
