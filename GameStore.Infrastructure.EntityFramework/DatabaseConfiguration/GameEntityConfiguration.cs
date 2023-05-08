using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using GameStore.Core.Entities;
using System.Reflection.Emit;

namespace GameStore.Infrastructure.EntityFramework.DatabaseConfiguration
{
    public class GameEntityConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> modelbuilder)
        {
            modelbuilder
                .HasKey(x => x.Id);

            modelbuilder
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelbuilder
                .Property(x => x.Name)
                .IsRequired();

            modelbuilder
                .Property(x => x.PublishDate)
                .IsRequired();

            modelbuilder
                .Property(x => x.Developer)
                .IsRequired(false);

            modelbuilder
                .Property(x => x.AvailableCopies)
                .IsRequired();

            modelbuilder
                .Property(x => x.GameMode)
                .IsRequired();

            modelbuilder
                .HasOne(x => x.Category)
                .WithMany(x => x.Games)
                .HasForeignKey(x => x.CategoryId);
        }
    }
}
