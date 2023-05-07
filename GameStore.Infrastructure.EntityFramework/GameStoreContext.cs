using GameStore.Core.Entities;
using Microsoft.EntityFrameworkCore;
using GameStore.Infrastructure.EntityFramework.DatabaseConfiguration;

namespace SocialNetwork.Infrastructure.EntityFramework
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
            //Configuracion del Usuario.
            modelBuilder.ApplyConfiguration<Category>(new CategoryEntityConfiguration());

            //Configuracion de Post
            modelBuilder.ApplyConfiguration<Game>(new GameEntityConfiguration());

            //Configuracion de Comentario
        }
    }
}
