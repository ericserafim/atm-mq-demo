namespace ATM.MQ.Core.Interfaces.Configuration
{
    public interface IMQConnectionSettings
    {
        string HostName { get; set; }

        int Port { get; set; }

        string UserName { get; set; }

        string Password { get; set; }

        string VirtualHost { get; set; }

        string Exchange { get; set; }

        string RountingKey { get; set; }

        string QueueName { get; set; }
    }
}
