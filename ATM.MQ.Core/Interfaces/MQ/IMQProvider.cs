using ATM.MQ.Core.Entities;
using System;

namespace ATM.MQ.Core.Interfaces.MQ
{
  public interface IMQProvider
  {
    event EventHandler<MessageData<Transaction>> OnReceivedMessage;

    void Connect();

    void PublishMessage(string senderId, MessageData<Transaction> message);

    void SubscribeQueue(string queueName);

    void Close();
  }
}
