using System.Threading.Tasks;
using ATM.MQ.Core.Interfaces.Configuration;
using ATM.MQ.Core.Interfaces.MQ;
using ATM.MQ.Core.Entities;
using RabbitMQ.Client;
using System.Text;
using ATM.MQ.Extensions;
using RabbitMQ.Client.Framing;
using RabbitMQ.Client.Events;
using System;

namespace ATM.MQ.RabbitMQ
{
    public class RabbitMQProvider : IMQProvider
    {
        private readonly IMQConnectionSettings _connectionSettings;

        private readonly ConnectionFactory _connectionFactory;

        private IConnection _connection;
        
        private IModel _channel;

        public event EventHandler<MessageData<Transaction>> OnReceivedMessage;

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

        public void PublishMessage(string senderId, MessageData<Transaction> message)
        {
            var properties = _channel.CreateBasicProperties();
            properties.AppId = senderId;
            properties.MessageId = message.Id;

            _channel.BasicPublish(exchange: _connectionSettings.Exchange,
                                  routingKey: _connectionSettings.RountingKey,
                                  basicProperties: properties,
                                  body: Encoding.UTF8.GetBytes(message.ToJson()));
        }

        public void SubscribeQueue(string queueName)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (sender, ea) =>
            {
                var messageJson = Encoding.UTF8.GetString(ea.Body);
                var message = messageJson.ToObject<MessageData<Transaction>>();

                this.OnReceivedMessage?.Invoke(sender, message);

                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(queue: queueName,
                                  autoAck: false,
                                  consumer: consumer);
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
