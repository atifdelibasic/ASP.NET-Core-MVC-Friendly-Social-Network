using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FriendlyRS1.Repository.RepositorySetup
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(Expression<Func<TEntity, bool>> predicate, string[] entities);
        IEnumerable<TEntity> GetAll(string[] objects);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        void Remove(TEntity entity);
    }
}
