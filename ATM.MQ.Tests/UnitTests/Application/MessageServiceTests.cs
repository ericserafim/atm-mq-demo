using System;
using Xunit;
using FluentAssertions;
using AutoFixture;
using Moq;
using ATM.MQ.Core.Entities;
using ATM.MQ.Core.Interfaces.Repositories;
using System.Threading.Tasks;
using ATM.MQ.Application.Services;
using ATM.MQ.Application.Exceptions;
using ATM.MQ.Core.Interfaces.MQ;

namespace ATM.MQ.Tests.UnitTests.Application
{
    public class MessageServiceTests
    {
        [Fact]
        public async Task SendMessage_Should_Send_Message()
        {
            //Arrange
            var fixture = new Fixture();
            var message = fixture.Create<MessageData<Transaction>>();
            var factoryMock = new Mock<IMQProviderFactory>();
            var providerMock = new Mock<IMQProvider>();
            var repoMock = new Mock<IMessageRepository<MessageData<Transaction>>>();


            providerMock.Setup(s => s.ConnectAsync()).Verifiable();
            providerMock.Setup(s => s.PublishMessageAsync(It.IsAny<string>(), message)).Verifiable();
            factoryMock.Setup(s => s.Create()).Returns(providerMock.Object);

            repoMock.Setup(s => s.SaveMessageAsync(It.IsAny<MessageData<Transaction>>()))
            .Returns(Task.FromResult(true));

            var sut = new MessageService(factoryMock.Object, repoMock.Object);

            //Action
            var result = await sut.SendMessageAsync(queueName: string.Empty, message);

            //Assert    
            result.Should().BeTrue();
            providerMock.Verify(s => s.ConnectAsync(), Times.Once);
            providerMock.Verify(s => s.PublishMessageAsync(It.IsAny<string>(), message), Times.Once);
            repoMock.Verify(x => x.SaveMessageAsync(It.IsAny<MessageData<Transaction>>()), Times.Once);
        }

        [Fact]
        public void SendMessage_Should_ThrowException_When_MessageIsNull()
        {
            //Arrange            
            MessageData<Transaction> message = null;
            var factoryMock = new Mock<IMQProviderFactory>();
            var providerMock = new Mock<IMQProvider>();
            var repoMock = new Mock<IMessageRepository<MessageData<Transaction>>>();
            factoryMock.Setup(s => s.Create()).Returns(providerMock.Object);
            var sut = new MessageService(factoryMock.Object, repoMock.Object);

            //Action
            Func<Task> result = async () => await sut.SendMessageAsync(queueName: string.Empty, message);

            //Assert    
            result.Should().Throw<InvalidMessageDataException>();
        }

        [Fact]
        public void SubscribeQueue_Should_Subscribe()
        {
            //Arrange                    
            var factoryMock = new Mock<IMQProviderFactory>();
            var providerMock = new Mock<IMQProvider>();
            var repoMock = new Mock<IMessageRepository<MessageData<Transaction>>>();
            providerMock.Setup(s => s.ConnectAsync()).Verifiable();
            providerMock.Setup(s => s.SubscribeQueueAsync(It.IsAny<string>())).Verifiable();
            factoryMock.Setup(s => s.Create()).Returns(providerMock.Object);
            var sut = new MessageService(factoryMock.Object, repoMock.Object);

            //Action
            Func<Task> result = async () => await sut.SubscribeQueueAsync(queueName: "name");

            //Assert    
            result.Should().NotThrow();
            providerMock.Verify(s => s.ConnectAsync(), Times.Once);
            providerMock.Verify(s => s.SubscribeQueueAsync(It.IsAny<string>()), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void SubscribeQueue_Should_ThrowException_When_QueueNameIsNullOrEmptyOrWhiteSpace(string queueName)
        {
            //Arrange                    
            var factoryMock = new Mock<IMQProviderFactory>();        
            var sut = new MessageService(factoryMock.Object, default);

            //Action
            Func<Task> result = async () => await sut.SubscribeQueueAsync(queueName);

            //Assert    
            result.Should().Throw<ArgumentNullException>();
        }
    }
}
