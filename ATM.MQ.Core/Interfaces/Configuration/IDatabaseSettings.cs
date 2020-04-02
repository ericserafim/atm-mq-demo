namespace ATM.MQ.Core.Interfaces.Configuration
{
    public interface IDatabaseSettings
    {      
        string ConnectionString { get; set; }

        string DatabaseName { get; set; }

        string AccountsCollectionName { get; set; }

        string OwnersCollectionName { get; set; }

        string MessagesCollectionName { get; set; }
    }
}
