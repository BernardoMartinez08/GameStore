using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Core.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Add(TEntity entity);
        TEntity Update(TEntity entity);
        void Delete(TEntity entity);
        TEntity GetById(int id);
        IReadOnlyList<TEntity> Get();
        IReadOnlyList<TEntity> Filter(Func<TEntity, bool> predicate);
    }
}
