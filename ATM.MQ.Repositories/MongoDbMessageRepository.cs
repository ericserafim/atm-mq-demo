using ATM.MQ.Core.Entities;
using ATM.MQ.Core.Interfaces.Configuration;
using ATM.MQ.Core.Interfaces.Repositories;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace ATM.MQ.Repositories
{
  public class MongoDbMessageRepository : IMessageRepository
  {
    private readonly IMongoCollection<MessageData<Transaction>> _messages;

    public MongoDbMessageRepository(IDatabaseSettings settings)
    {
      var client = new MongoClient(settings.ConnectionString);
      var database = client.GetDatabase(settings.DatabaseName);

      _messages = database.GetCollection<MessageData<Transaction>>(settings.MessagesCollectionName);
    }

    public async Task<bool> DeleteMessageAsync(string id)
    {
      var result = await _messages.DeleteOneAsync(m => m.Id.Equals(id));
      return result.DeletedCount == 1;
    }

    public async Task<MessageData<Transaction>> GetMessageAsync(string id)
    {
      var result = await _messages.FindAsync(m => m.Id.Equals(id));
      return result.FirstOrDefault();
    }

    public async Task<bool> SaveMessageAsync(MessageData<Transaction> message)
    {
      await _messages.InsertOneAsync(message);
      return true;
    }
  }
}
