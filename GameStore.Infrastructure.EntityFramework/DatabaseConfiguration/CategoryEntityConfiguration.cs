using GameStore.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Infrastructure.EntityFramework.DatabaseConfiguration
{
    public class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> modelBuilder)
        {
            modelBuilder
                .HasKey(x => x.Id);

            modelBuilder
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelBuilder
                .Property(x => x.Name)
                .IsRequired();

            modelBuilder
                .HasMany(x => x.Games)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryId);

        }
    }
}
