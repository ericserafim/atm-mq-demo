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
		[Fact]
		public void SendMessage_Should_Send_Message()
		{
			//Arrange
			var fixture = new Fixture();
			var message = fixture.Create<MessageData<Transaction>>();
			var factoryMock = new Mock<IMQProviderFactory>();
			var providerMock = new Mock<IMQProvider>();
			var repoMock = new Mock<IMessageRepository>();


			providerMock.Setup(s => s.Connect()).Verifiable();
			providerMock.Setup(s => s.PublishMessage(It.IsAny<string>(), message)).Verifiable();
			factoryMock.Setup(s => s.Create()).Returns(providerMock.Object);

			repoMock.Setup(s => s.SaveMessageAsync(It.IsAny<MessageData<Transaction>>()))
			.Returns(Task.FromResult(true));

			var sut = new MessageService(factoryMock.Object, repoMock.Object);

			//Action
			Func<Task> result = async () => await sut.SendMessageAsync(senderId: Guid.NewGuid().ToString(), message);

			//Assert    
			result.Should().NotThrow();
			providerMock.Verify(s => s.Connect(), Times.Once);
			providerMock.Verify(s => s.PublishMessage(It.IsAny<string>(), message), Times.Once);			
		}

		[Fact]
		public void SendMessage_Should_ThrowException_When_MessageIsNull()
		{
			//Arrange            
			MessageData<Transaction> message = null;
			var factoryMock = new Mock<IMQProviderFactory>();
			var providerMock = new Mock<IMQProvider>();
			var repoMock = new Mock<IMessageRepository>();
			factoryMock.Setup(s => s.Create()).Returns(providerMock.Object);
			var sut = new MessageService(factoryMock.Object, repoMock.Object);

			//Action
			Func<Task> result = async () => await sut.SendMessageAsync(senderId: Guid.NewGuid().ToString(), message);

			//Assert    
			result.Should().Throw<InvalidMessageDataException>();
		}

		[Fact]
		public void SubscribeQueue_Should_Subscribe()
		{
			//Arrange                    
			var factoryMock = new Mock<IMQProviderFactory>();
			var providerMock = new Mock<IMQProvider>();
			var repoMock = new Mock<IMessageRepository>();
			providerMock.Setup(s => s.Connect()).Verifiable();
			providerMock.Setup(s => s.SubscribeQueue(It.IsAny<string>())).Verifiable();
			factoryMock.Setup(s => s.Create()).Returns(providerMock.Object);
			var sut = new MessageService(factoryMock.Object, repoMock.Object);

			//Action
			Func<Task> result = async () => await sut.SubscribeQueueAsync(queueName: "name");

			//Assert    
			result.Should().NotThrow();
			providerMock.Verify(s => s.Connect(), Times.Once);
			providerMock.Verify(s => s.SubscribeQueue(It.IsAny<string>()), Times.Once);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		public void SubscribeQueue_Should_ThrowException_When_QueueNameIsNullOrEmptyOrWhiteSpace(string queueName)
		{
			//Arrange                    
			var providerMock = new Mock<IMQProvider>();
			var factoryMock = new Mock<IMQProviderFactory>();
			factoryMock.Setup(s => s.Create()).Returns(providerMock.Object);
			var sut = new MessageService(factoryMock.Object, default);

			//Action
			Func<Task> result = async () => await sut.SubscribeQueueAsync(queueName);

			//Assert    
			result.Should().Throw<ArgumentNullException>();
		}
	}
}
