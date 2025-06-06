using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_stock.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api_stock.Data
{
    public class ApplicationDBContext : IdentityDbContext<User>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Container> Containers { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Place> Places { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Container>().HasIndex(c => c.PlaceId);
            builder.Entity<Container>().HasIndex(c => c.ParentContainerId);
            builder.Entity<Item>().HasIndex(i => i.ContainerId);

            builder.Entity<Container>()
                .HasMany(c => c.Items)
                .WithOne(i => i.Container)
                .HasForeignKey(i => i.ContainerId);

            builder.Entity<Item>()
                .HasOne(i => i.Container)
                .WithMany(c => c.Items)
                .HasForeignKey(i => i.ContainerId);

            builder.Entity<Container>()
                .HasOne(p => p.Place)
                .WithMany(c => c.Containers)
                .HasForeignKey(c => c.PlaceId);

            builder.Entity<Container>()
                .HasMany(c => c.Containers)
                .WithOne(c => c.ParentContainer)
                .HasForeignKey(c => c.ParentContainerId);

        }
    }
}