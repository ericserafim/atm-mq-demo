using System;
using ATM.MQ.Core.Entities;
using ATM.MQ.Core.Interfaces.Configuration;
using ATM.MQ.Core.Interfaces.Repositories;
using MongoDB.Driver;

namespace ATM.MQ.Repositories
{
    public class MongoDbContext : IContextRepository
    {
        private readonly IDatabaseSettings _settings;

        private readonly IClientSessionHandle _transaction;

        private readonly IMongoDatabase _database;



        public MongoDbContext(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
            _settings = settings;

            _transaction = client.StartSession();        
        }

        public IGenericRepository<Account> Accounts => 
            Accounts ?? new MongoDbGenericRepository<Account>(_database, _settings.AccountsCollectionName);

        public IGenericRepository<Owner> Owners => 
            Owners ?? new MongoDbGenericRepository<Owner>(_database, _settings.OwnersCollectionName);

        public IGenericRepository<MessageData<Transaction>> Messages =>
            Messages ?? new MongoDbGenericRepository<MessageData<Transaction>>(_database, _settings.MessagesCollectionName);

        public void SaveChanges()
        {
            //TODO Should thing how to track changes in Repositories to start transaction
            try
            {
                _transaction.CommitTransaction();                
            }
            catch 
            {
                _transaction.AbortTransaction();
                throw;
            } 
        }
    }
}
