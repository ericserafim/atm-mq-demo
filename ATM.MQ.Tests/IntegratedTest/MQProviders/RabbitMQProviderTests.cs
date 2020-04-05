using ATM.MQ.Core.Entities;
using ATM.MQ.RabbitMQ;
using ATM.MQ.Extensions;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace ATM.MQ.Tests.IntegratedTest.MQProviders
{
	public class RabbitMQProviderTests
	{
		private readonly RabbitMQProvider<MessageData<Transaction>> _provider;

    private readonly ITestOutputHelper _output;

    private readonly IConfigurationRoot _configuration;

    public RabbitMQProviderTests(ITestOutputHelper output)
		{
			_configuration = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json", optional: false)
			.Build();

			var settings = _configuration.GetSection(nameof(MQConnectionSettings)).Get<MQConnectionSettings>();

			_provider = new RabbitMQProvider<MessageData<Transaction>>(settings);
      this._output = output;
    }

		[Fact]
		public void Connect_Should_Connect()
		{
			//Act
			Action result = () => _provider.Connect();

			//Assert
			result.Should().NotThrow();
		}

		[Fact]
		public void PublishMessage_Should_PublishMessage()
		{
			//Arrange
			var fixture = new Fixture();
			var message = fixture.Create<MessageData<Transaction>>();
			var exchange = _configuration["AppSettings:Exchange"];
			var rountingKey = _configuration["AppSettings:RoutingKey"];
			_output.WriteLine(message.ToJson());

			//Act
			_provider.Connect();
			Action result = () => _provider.PublishMessage(exchange, rountingKey, senderId: Guid.NewGuid().ToString(), message);

			//Assert
			result.Should().NotThrow();
		}

		[Fact]
		public void SubscribeQueue_Should_SubscribeQueue()
		{
			//Arrange
			var queueName = _configuration["AppSettings:QueueName"];
			
			//Act
			_provider.Connect();
			Action result = () => _provider.SubscribeQueue(queueName);

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
			var queueName = _configuration["AppSettings:QueueName"];
			var exchange = _configuration["AppSettings:Exchange"];
			var rountingKey = _configuration["AppSettings:RoutingKey"];

			_provider.Connect();
			_provider.SubscribeQueue(queueName);

			_provider.OnReceivedMessage += (sender, msg) =>
			{
				messageReceived = (MessageData<Transaction>)msg;
				_autoResetEvent.Set();
			};

			//Act
			_provider.PublishMessage(exchange, rountingKey, senderId: Guid.NewGuid().ToString(), messageSent);

			//Assert
			_autoResetEvent.WaitOne(8000).Should().BeTrue();
			messageReceived.Should().BeEquivalentTo(messageSent);
		}
	}
}
