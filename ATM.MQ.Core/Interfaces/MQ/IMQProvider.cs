using System;

namespace ATM.MQ.Core.Interfaces.MQ
{
  public interface IMQProvider<T>
	{
		event EventHandler<T> OnReceivedMessage;

		void Connect();

		void PublishMessage(string exchange, string routingKey, string senderId, object message);

		void SubscribeQueue(string queueName);

		void Close();    
  }
}
