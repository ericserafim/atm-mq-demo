using ATM.MQ.Core.Entities;
using ATM.MQ.Core.Interfaces.Configuration;
using ATM.MQ.Core.Interfaces.Repositories;
using MongoDB.Driver;

namespace ATM.MQ.Repositories
{
  public class MongoDbContext : IContextRepository
  {
    private readonly IDatabaseSettings _settings;
    
    private readonly IMongoDatabase _database;

    private MongoDbGenericRepository<Owner> _owners;

    private MongoDbGenericRepository<MessageData<Transaction>> _messages;
    private MongoDbGenericRepository<Account> _accounts;

    public MongoDbContext(IDatabaseSettings settings)
    {
      _settings = settings;

      var client = new MongoClient(_settings.ConnectionString);
      _database = client.GetDatabase(_settings.DatabaseName);        
    }

    public IGenericRepository<Account> Accounts =>
        _accounts = _accounts ?? new MongoDbGenericRepository<Account>(_database, _settings.AccountsCollectionName);

    public IGenericRepository<Owner> Owners =>
        _owners = _owners ?? new MongoDbGenericRepository<Owner>(_database, _settings.OwnersCollectionName);

    public IGenericRepository<MessageData<Transaction>> Messages =>
        _messages = _messages ?? new MongoDbGenericRepository<MessageData<Transaction>>(_database, _settings.MessagesCollectionName);

    public void SaveChanges()
    {
      //TODO I need to thing how to track changes in Repositories to start and commit transaction      
    }
  }
}
