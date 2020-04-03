using ATM.MQ.Application.Exceptions;
using ATM.MQ.Application.Services;
using ATM.MQ.Core.Entities;
using ATM.MQ.Core.Interfaces.MQ;
using ATM.MQ.Core.Interfaces.Repositories;
using AutoFixture;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ATM.MQ.Tests.UnitTests.Application
{
	public class MessageServiceTests
	{
		private readonly MessageService _messageService;

		private readonly Mock<IMQProvider> _providerMock;

		public MessageServiceTests()
		{
			var factoryMock = new Mock<IMQProviderFactory>();
			_providerMock = new Mock<IMQProvider>();
			var dbContextMock = new Mock<IContextRepository>();

			_providerMock.Setup(s => s.Connect()).Verifiable();
			_providerMock.Setup(s => s.PublishMessage(It.IsAny<string>(), It.IsAny<MessageData<Transaction>>())).Verifiable();
			_providerMock.Setup(s => s.SubscribeQueue(It.IsAny<string>())).Verifiable();
			factoryMock.Setup(s => s.Create()).Returns(_providerMock.Object);
			dbContextMock.SetupAllProperties();

			_messageService = new MessageService(factoryMock.Object, dbContextMock.Object);
		}

		[Fact]
		public void SendMessage_Should_Send_Message()
		{
			//Arrange
			var fixture = new Fixture();
			var message = fixture.Create<MessageData<Transaction>>();

			//Action
			Func<Task> result = async () => await _messageService.SendMessageAsync(senderId: Guid.NewGuid().ToString(), message);

			//Assert    
			result.Should().NotThrow();
			_providerMock.Verify(s => s.Connect(), Times.Once);
			_providerMock.Verify(s => s.PublishMessage(It.IsAny<string>(), message), Times.Once);
		}

		[Fact]
		public void SendMessage_Should_ThrowException_When_MessageIsNull()
		{
			//Arrange            
			MessageData<Transaction> message = null;

			//Action
			Func<Task> result = async () => await _messageService.SendMessageAsync(senderId: Guid.NewGuid().ToString(), message);

			//Assert    
			result.Should().Throw<InvalidMessageDataException>();
		}

		[Fact]
		public void SubscribeQueue_Should_Subscribe()
		{
			//Action
			Func<Task> result = async () => await _messageService.SubscribeQueueAsync(queueName: "name");

			//Assert    
			result.Should().NotThrow();
			_providerMock.Verify(s => s.Connect(), Times.Once);
			_providerMock.Verify(s => s.SubscribeQueue(It.IsAny<string>()), Times.Once);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		public void SubscribeQueue_Should_ThrowException_When_QueueNameIsNullOrEmptyOrWhiteSpace(string queueName)
		{			
			//Action
			Func<Task> result = async () => await _messageService.SubscribeQueueAsync(queueName);

			//Assert    
			result.Should().Throw<ArgumentNullException>();
		}
	}
}
