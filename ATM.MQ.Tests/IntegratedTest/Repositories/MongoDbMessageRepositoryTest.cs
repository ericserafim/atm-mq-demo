﻿using System;
using System.Threading.Tasks;
using ATM.MQ.Core.Entities;
using ATM.MQ.Repositories;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace ATM.MQ.Tests.IntegratedTest.Repositories
{
	public class MongoDbMessageRepositoryTest
	{
		private readonly MongoDbMessageRepository _repository;

		public MongoDbMessageRepositoryTest()
		{
			var config = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json", optional: false)
			.Build();

			var settings = config.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();

			_repository = new MongoDbMessageRepository(settings);
		}

        [Fact]
		public async Task SaveMessageAsync_Should_InsertOneMessage()
		{
			//Arrange
			var fixture = new Fixture();
			var message = fixture.Create<MessageData<Transaction>>();			

			//Act
			var result = await _repository.SaveMessageAsync(message);

			//Assert
			result.Should().BeTrue();
		}


        [Theory]
		[InlineData("907aa006-b499-468d-a68b-703bb39d87a9")]	
		[InlineData("abbcdb2f-03b9-44b0-aea1-8502aab37df2")]	
		public async Task GetMessageAsync_Should_GetMessage(string messageId)
		{
			//Act
			var result = await _repository.GetMessageAsync(messageId);

			//Assert
			result.Id.Should().Be(messageId);			
		}

		[Fact]
		public async Task DeleteMessageAsync_Should_DeleteOneMessage()
		{
			//Arrange
			var fixture = new Fixture();
			var message = fixture.Create<MessageData<Transaction>>();						
			_ = await _repository.SaveMessageAsync(message);

			//Act
			var result = await _repository.DeleteMessageAsync(message.Id);

			//Assert
			result.Should().BeTrue();
		}
	}
}