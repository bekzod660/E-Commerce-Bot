﻿using E_Commerce_Bot.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Bot.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categorys { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(e => e.Id)
                .IsUnique();
            modelBuilder.Entity<User>()
               .HasIndex(e => e.PhoneNumber)
               .IsUnique();

            modelBuilder.Entity<User>()
                .HasMany<Order>(x => x.Orders)
                .WithOne(e => e.User)
                .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<User>()
                .HasOne<Cart>(x => x.Cart)
                .WithOne(y => y.User);

            base.OnModelCreating(modelBuilder);
        }
    }
}
