using System;
using System.Threading;
using System.Threading.Tasks;
using ATM.MQ.Core.Entities;
using ATM.MQ.RabbitMQ;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace ATM.MQ.Tests.IntegratedTest.MQProviders
{
    public class RabbitMQProviderTests
    {
        private readonly RabbitMQProvider _provider;

        public RabbitMQProviderTests()
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

            var settings = config.GetSection(nameof(MQConnectionSettings)).Get<MQConnectionSettings>();

            this._provider = new RabbitMQProvider(settings);
        }

        [Fact]
        public void Connect_Should_Connect()
        {
            //Act
            Action result = () => this._provider.Connect();

            //Assert
            result.Should().NotThrow();
        }

        [Fact]
        public void PublishMessage_Should_PublishMessage()
        {
            //Arrange
            var fixture = new Fixture();
            var message = fixture.Create<MessageData<Transaction>>();

            //Act
            this._provider.Connect();
            Action result = () => this._provider.PublishMessage(senderId: Guid.NewGuid().ToString(), message);

            //Assert
            result.Should().NotThrow();
        }

        [Fact]
        public void SubscribeQueue_Should_SubscribeQueue()
        {
            //Act
            this._provider.Connect();
            Action result = () => this._provider.SubscribeQueue(queueName: "atm-messages");

            //Assert
            result.Should().NotThrow();
        }

        [Fact]
        public void SubscribeQueue_Should_ReceiveMessage()
        {
            //Arrange
            var _autoResetEvent = new AutoResetEvent(false);
            var fixture = new Fixture();
            var messageSent = fixture.Create<MessageData<Transaction>>();
            MessageData<Transaction> messageReceived = null;

            this._provider.Connect();
            this._provider.SubscribeQueue(queueName: "atm-messages");

            this._provider.OnReceivedMessage += (sender, msg) =>
            {
                messageReceived = msg;
                _autoResetEvent.Set();
            };

            //Act
            this._provider.PublishMessage(senderId: Guid.NewGuid().ToString(), messageSent);

            //Assert
            _autoResetEvent.WaitOne(8000).Should().BeTrue();
            messageReceived.Should().BeEquivalentTo(messageSent);
        }
    }
}