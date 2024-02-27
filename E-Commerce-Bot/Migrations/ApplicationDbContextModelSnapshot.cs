﻿// <auto-generated />
using System;
using E_Commerce_Bot.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace E_Commerce_Bot.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("E_Commerce_Bot.Entities.Cart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("E_Commerce_Bot.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("E_Commerce_Bot.Entities.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("CartsId")
                        .HasColumnType("integer");

                    b.Property<int>("Count")
                        .HasColumnType("integer");

                    b.Property<int?>("ProductId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CartsId");

                    b.HasIndex("ProductId");

                    b.ToTable("Item");
                });

            modelBuilder.Entity("E_Commerce_Bot.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<int?>("Cart")
                        .HasColumnType("integer");

                    b.Property<int?>("CartsId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Longitute")
                        .HasColumnType("numeric");

                    b.Property<int>("OrderType")
                        .HasColumnType("integer");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CartsId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("E_Commerce_Bot.Entities.ProcessHelper", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<double?>("Latitude")
                        .HasColumnType("double precision");

                    b.Property<double?>("Longitute")
                        .HasColumnType("double precision");

                    b.Property<int?>("Order")
                        .HasColumnType("integer");

                    b.Property<int?>("ProductId")
                        .HasColumnType("integer");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("ProcessHelpers");
                });

            modelBuilder.Entity("E_Commerce_Bot.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("E_Commerce_Bot.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int?>("CartId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<int?>("ProcessHelperId")
                        .HasColumnType("integer");

                    b.Property<int>("UserProcess")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("PhoneNumber")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("E_Commerce_Bot.Entities.Cart", b =>
                {
                    b.HasOne("E_Commerce_Bot.Entities.User", "User")
                        .WithOne("Cart")
                        .HasForeignKey("E_Commerce_Bot.Entities.Cart", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("E_Commerce_Bot.Entities.Item", b =>
                {
                    b.HasOne("E_Commerce_Bot.Entities.Cart", "Carts")
                        .WithMany("Items")
                        .HasForeignKey("CartsId");

                    b.HasOne("E_Commerce_Bot.Entities.Product", "Product")
                        .WithMany("Items")
                        .HasForeignKey("ProductId");

                    b.Navigation("Carts");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("E_Commerce_Bot.Entities.Order", b =>
                {
                    b.HasOne("E_Commerce_Bot.Entities.Cart", "Carts")
                        .WithMany()
                        .HasForeignKey("CartsId");

                    b.HasOne("E_Commerce_Bot.Entities.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Carts");

                    b.Navigation("User");
                });

            modelBuilder.Entity("E_Commerce_Bot.Entities.ProcessHelper", b =>
                {
                    b.HasOne("E_Commerce_Bot.Entities.User", "User")
                        .WithOne("ProcessHelper")
                        .HasForeignKey("E_Commerce_Bot.Entities.ProcessHelper", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("E_Commerce_Bot.Entities.Product", b =>
                {
                    b.HasOne("E_Commerce_Bot.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("E_Commerce_Bot.Entities.Cart", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("E_Commerce_Bot.Entities.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("E_Commerce_Bot.Entities.Product", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("E_Commerce_Bot.Entities.User", b =>
                {
                    b.Navigation("Cart");

                    b.Navigation("Orders");

                    b.Navigation("ProcessHelper");
                });
#pragma warning restore 612, 618
        }
    }
}
