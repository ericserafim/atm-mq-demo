using ATM.MQ.Core.Entities;
using ATM.MQ.Repositories;
using Xunit;

namespace ATM.MQ.Tests.IntegratedTest.Repositories
{
	public class MongoDbMessageRepositoryTest
	{
		private readonly MongoDbMessageRepository _repository;

		public MongoDbMessageRepositoryTest()
		{
			var settings = new DatabaseSettings 
			{
				ConnectionString = "",
				DatabaseName = "",
				MessagesCollectionName = ""
			};

			_repository = new MongoDbMessageRepository(settings);
		}

		[Fact]
		public void Should_InsertOneMessage()
		{
			//Arrange

			//Act

			//Assert

		}
	}
}
