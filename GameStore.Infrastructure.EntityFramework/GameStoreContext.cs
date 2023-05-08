using GameStore.Core.Entities;
using Microsoft.EntityFrameworkCore;
using GameStore.Infrastructure.EntityFramework.DatabaseConfiguration;

namespace GameStore.Infrastructure.EntityFramework
{
    public class GameStoreContext : DbContext
    {
        public GameStoreContext(DbContextOptions<GameStoreContext> options) : base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Category>(new CategoryEntityConfiguration());
            modelBuilder.ApplyConfiguration<Game>(new GameEntityConfiguration());
        }
    }
}
