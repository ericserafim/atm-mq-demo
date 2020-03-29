using System;
using ATM.MQ.Core.Interfaces.Configuration;

namespace ATM.MQ.Core.Entities
{
    public class MQConnectionSettings : IMQConnectionSettings
    {
        public string HostName { get; set; }

        public int Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string VirtualHost { get; set; }

        public string Exchange { get; set; }
        
        public string RountingKey { get; set; }

        public string QueueName { get; set; }
    }
}
