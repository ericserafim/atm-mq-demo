namespace ATM.MQ.Core.Interfaces.Configuration
{
  public interface IDatabaseSettings
  {
    string MessagesCollectionName { get; set; }

    string ConnectionString { get; set; }

    string DatabaseName { get; set; }
  }
}
