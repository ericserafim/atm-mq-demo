using ATM.MQ.Application.Exceptions;
using ATM.MQ.Application.Services;
using ATM.MQ.Core.Entities;
using ATM.MQ.Core.Interfaces.Configuration;
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

    private readonly Mock<IMQProvider<MessageData<Transaction>>> _providerMock;

    private readonly Mock<IAppSettings> _appSettings;

    public MessageServiceTests()
    {
      var dbContextMock = new Mock<IContextRepository>();
      _appSettings = new Mock<IAppSettings>();

      _providerMock = new Mock<IMQProvider<MessageData<Transaction>>>();
      _providerMock.Setup(s => s.Connect()).Verifiable();
      _providerMock.Setup(s => s.PublishMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageData<Transaction>>())).Verifiable();
      _providerMock.Setup(s => s.SubscribeQueue(It.IsAny<string>())).Verifiable();

      dbContextMock.SetupAllProperties();


      _messageService = new MessageService(_providerMock.Object, dbContextMock.Object, _appSettings.Object);
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
      _providerMock.Verify(s => s.PublishMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), message), Times.Once);
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
      //Arragnge
      _appSettings.SetupProperty(f => f.QueueName, "Test");

      //Action
      Func<Task> result = async () => await _messageService.SubscribeQueueAsync();

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
      //Arrange
      _appSettings.SetupProperty(f => f.QueueName, queueName);

      //Action
      Func<Task> result = async () => await _messageService.SubscribeQueueAsync();

      //Assert    
      result.Should().Throw<ArgumentNullException>();
    }
  }
}
