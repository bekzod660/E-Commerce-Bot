﻿using E_Commerce_Bot.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Bot.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProcessHelper> ProcessHelpers { get; set; }

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
                .HasMany(x => x.Orders)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(x => x.Basket)
                .WithOne(y => y.User)
                .HasForeignKey<Basket>(y => y.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(x => x.ProcessHelper)
                .WithOne(y => y.User)
                .HasForeignKey<ProcessHelper>(y => y.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);


            base.OnModelCreating(modelBuilder);
        }
    }
}
