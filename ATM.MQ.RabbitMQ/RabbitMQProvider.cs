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
            this._connectionSettings = connectionSettings;

            this._connectionFactory = new ConnectionFactory()
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
            if (this._connection != null)
                return;

            this._connection = this._connectionFactory.CreateConnection();
            this._channel = this._connection.CreateModel();
        }

        public void PublishMessage(string senderId, MessageData<Transaction> message)
        {
            var properties = this._channel.CreateBasicProperties();
            properties.AppId = senderId;
            properties.MessageId = message.Id;

            this._channel.BasicPublish(exchange: this._connectionSettings.Exchange,
                                routingKey: this._connectionSettings.RountingKey,
                                basicProperties: properties,
                                body: Encoding.UTF8.GetBytes(message.ToJson()));
        }

        public void SubscribeQueue(string queueName)
        {
            var consumer = new EventingBasicConsumer(this._channel);

            consumer.Received += (sender, ea) =>
            {
                var messageJson = Encoding.UTF8.GetString(ea.Body);
                var message = messageJson.ToObject<MessageData<Transaction>>();

                this.OnReceivedMessage?.Invoke(sender, message);

                this._channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            this._channel.BasicConsume(queue: queueName,
                                 autoAck: false,
                                 consumer: consumer);
        }

        public void Close()
        {
            this._channel?.Close();
            this._channel?.Dispose();
            
            this._connection?.Close();
            this._connection?.Dispose();
        }
    }
}
