using ATM.MQ.Core.Entities;
using ATM.MQ.Core.Interfaces.Repositories;
using ATM.MQ.Repositories;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ATM.MQ.Tests.IntegratedTest.Repositories
{
	public class MongoDbMessageRepositoryTest
	{
		private readonly IContextRepository _dbContext;

		public MongoDbMessageRepositoryTest()
		{
			var config = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json", optional: false)
			.Build();

			var settings = config.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();

			_dbContext = new MongoDbContext(settings);
		}

		[Fact]
		public void Should_InsertOneMessage()
		{
			//Arrange
			var fixture = new Fixture();
			var message = fixture.Create<MessageData<Transaction>>();

			//Act
			Func<Task> result = async () => await _dbContext.Messages.InsertAsync(message);

			//Assert
			result.Should().NotThrow();
		}


		[Theory]
		[InlineData("Idaa212308-bef2-4b30-a196-f3d071a56526")]
		[InlineData("Id356ff97e-14b2-4537-b875-af1df382c6f0")]
		public async Task Should_GetMessage(string messageId)
		{
			//Act
			var result = await _dbContext.Messages.GetAsync(messageId);

			//Assert
			result.Id.Should().Be(messageId);
		}

		[Fact]
		public async Task DeleteMessageAsync_Should_DeleteOneMessage()
		{
			//Arrange
			var fixture = new Fixture();
			var message = fixture.Create<MessageData<Transaction>>();
			await _dbContext.Messages.InsertAsync(message);

			//Act
			Func<Task> result = async () => await _dbContext.Messages.DeleteAsync(message.Id);

			//Assert
			result.Should().NotThrow();
		}
	}
}
