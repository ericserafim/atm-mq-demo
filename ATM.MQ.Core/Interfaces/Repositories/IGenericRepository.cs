using System;

namespace ATM.MQ.Core.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        TEntity Get(object id);

        void Insert(TEntity entity);

        void Update(TEntity entity);

        void Delete(object id);
    }
}
