using ATM.MQ.Core.Interfaces.Configuration;
using ATM.MQ.Core.Interfaces.MQ;
using ATM.MQ.Extensions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace ATM.MQ.RabbitMQ
{
  public class RabbitMQProvider<T> : IMQProvider<T>
  {
    private readonly IMQConnectionSettings _connectionSettings;

    private readonly ConnectionFactory _connectionFactory;

    private IConnection _connection;

    private IModel _channel;

    public event EventHandler<T> OnReceivedMessage;

    public RabbitMQProvider(IMQConnectionSettings connectionSettings)
    {
      _connectionSettings = connectionSettings;

      _connectionFactory = new ConnectionFactory()
      {
        HostName = connectionSettings.HostName,
        Port = connectionSettings.Port,
        UserName = connectionSettings.UserName,
        Password = connectionSettings.Password,
        VirtualHost = connectionSettings.VirtualHost
      };
    }

    public void Connect()
    {
      if (_connection != null)
        return;

      _connection = _connectionFactory.CreateConnection();
      _channel = _connection.CreateModel();
    }

    public void PublishMessage(string exchange, string routingKey, string senderId, object message)
    {
      _channel.BasicPublish(exchange,
                            routingKey,
                            basicProperties: CreateProperties(senderId),
                            body: Encoding.UTF8.GetBytes(message.ToJson()));
    }

    public void SubscribeQueue(string queueName)
    {
      var consumer = new EventingBasicConsumer(_channel);

      consumer.Received += (sender, ea) =>
      {
        var message = Encoding.UTF8.GetString(ea.Body);
        this.OnReceivedMessage?.Invoke(sender, message.ToObject<T>());
        _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
      };

      _channel.BasicConsume(queue: queueName,
                            autoAck: false,
                            consumer: consumer);
    }

    private IBasicProperties CreateProperties(string senderId)
    {
      var properties = _channel.CreateBasicProperties();
      properties.AppId = senderId;

      return properties;
    }

    public void Close()
    {
      _channel?.Close();
      _channel?.Dispose();

      _connection?.Close();
      _connection?.Dispose();
    }
  }
}
