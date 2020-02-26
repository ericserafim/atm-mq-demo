using System;

namespace ATM.MQ.Core.Entities
{
    public class MessageData<T>
    {
        public long Id { get; set; }
        
        public T Body { get; set; }
        
        public string CreatedBy { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
