using GameStore.Core.Interfaces;

namespace GameStore.Infrastructure.EntityFramework.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly GameStoreContext context;
        public BaseRepository(GameStoreContext context)
        {
            this.context = context;
        }
        public TEntity Add(TEntity entity)
        {
            var result = context.Add(entity);
            context.SaveChanges();
            return result.Entity;
        }

        public IReadOnlyList<TEntity> Filter(Func<TEntity, bool> predicate)
        {
            return context.Set<TEntity>().Where(predicate).ToList();
        }

        public IReadOnlyList<TEntity> Get()
        {
            var entities = context.Set<TEntity>().ToList();
            return entities;
        }

        public TEntity GetById(int id)
        {
            var entities = context.Set<TEntity>().Find(id);
            return entities;
        }

        public TEntity Update(TEntity entity)
        {
            var result = context.Update(entity);
            context.SaveChanges();
            return result.Entity;
        }

        void IRepository<TEntity>.Delete(TEntity entity)
        {
            context.Remove(entity);
            context.SaveChanges();
        }
    }
}
