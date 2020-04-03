using ATM.MQ.Core.Interfaces.Configuration;

namespace ATM.MQ.Core.Entities
{
	public class DatabaseSettings : IDatabaseSettings
	{
		public string ConnectionString { get; set; }

		public string DatabaseName { get; set; }

		public string AccountsCollectionName { get; set; }

		public string OwnersCollectionName { get; set; }

		public string MessagesCollectionName { get; set; }
	}
}
