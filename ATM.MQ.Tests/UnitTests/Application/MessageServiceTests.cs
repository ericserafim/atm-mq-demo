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

namespace ATM.MQ.Tests.UnitTests.Application
{
    public class MessageServiceTests
    {
        [Fact]
        public async Task Should_Save_Message()
        {
            //Arrange
            var fixture = new Fixture();
            var message = fixture.Create<MessageData<Transaction>>();
            var repoMock = new Mock<IMessageRepository<MessageData<Transaction>>>();

            repoMock
            .Setup(s => s.SaveMessageAsync(It.IsAny<MessageData<Transaction>>()))
            .Returns(Task.FromResult(true));

            var sut = new MessageService(repoMock.Object);

            //Action
            var result = await sut.SaveMessageAsync(message);

            //Assert    
            result.Should().BeTrue();
            repoMock.Verify(x => x.SaveMessageAsync(It.IsAny<MessageData<Transaction>>()), Times.Once);
        }

        [Fact]
        public async Task Should_Return_Message()
        {
            //Arrange
            var fixture = new Fixture();
            var message = fixture.Create<MessageData<Transaction>>();
            var repoMock = new Mock<IMessageRepository<MessageData<Transaction>>>();

            repoMock
            .Setup(s => s.GetMessageAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(message));

            var sut = new MessageService(repoMock.Object);

            //Action
            var result = await sut.GetMessageAsync(message.Id);

            //Assert    
            result.Should().BeSameAs(message);
            repoMock.Verify(x => x.GetMessageAsync(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public async Task Should_Delete_Message()
        {
            //Arrange
            var fixture = new Fixture();
            var message = fixture.Create<MessageData<Transaction>>();
            var repoMock = new Mock<IMessageRepository<MessageData<Transaction>>>();
            
            repoMock
                .Setup(s => s.GetMessageAsync(It.IsAny<long>()))
                .Returns(Task.FromResult(message));

            repoMock
                .Setup(s => s.DeleteMessageAsync(It.IsAny<long>()))
                .Returns(Task.FromResult(true));

            var sut = new MessageService(repoMock.Object);

            //Action
            var result = await sut.DeleteMessageAsync(message.Id);

            //Assert    
            result.Should().BeTrue();
            repoMock.Verify(x => x.DeleteMessageAsync(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void SaveMessage_Should_Throw_Exception_When_MessageIsNull()
        {
            //Arrange            
            MessageData<Transaction> message = null;
            var repoMock = new Mock<IMessageRepository<MessageData<Transaction>>>();
            var sut = new MessageService(repoMock.Object);

            //Action
            Func<Task> result = async () => await sut.SaveMessageAsync(message);

            //Assert    
            result.Should().Throw<InvalidMessageDataException>();
        }

        [Fact]
        public void DeleteMessage_Should_Throw_Exception_When_MessageNotFound()
        {
            //Arrange            
            var fixture = new Fixture();
            var message = fixture.Create<MessageData<Transaction>>();
            var repoMock = new Mock<IMessageRepository<MessageData<Transaction>>>();

            repoMock.Setup(s => s.GetMessageAsync(It.IsAny<long>()))
                .Returns(Task.FromResult<MessageData<Transaction>>(null));

            var sut = new MessageService(repoMock.Object);

            //Action
            Func<Task> result = async () => await sut.DeleteMessageAsync(message.Id);

            //Assert    
            result.Should().Throw<DataNotFoundException>();
        }
    }
}
