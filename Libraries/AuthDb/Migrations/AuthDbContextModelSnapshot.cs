﻿// <auto-generated />
using System;
using AuthDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AuthDb.Migrations
{
    [DbContext(typeof(AuthDbContext))]
    partial class AuthDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("AuthDb")
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AuthDb.Account", b =>
                {
                    b.Property<string>("AccountId")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("AccountId");

                    b.ToTable("Accounts", "AuthDb");
                });

            modelBuilder.Entity("AuthDb.Foo", b =>
                {
                    b.Property<long>("Seq")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Seq"));

                    b.Property<string>("AccountId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Command")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.HasKey("Seq");

                    b.ToTable("Foos", "AuthDb");
                });

            modelBuilder.Entity("AuthDb.Maintenance", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("End")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Start")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Maintenances", "AuthDb");
                });

            modelBuilder.Entity("AuthDb.Password", b =>
                {
                    b.Property<string>("AccountId")
                        .HasColumnType("text");

                    b.Property<string>("AccountPassword")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("AccountId");

                    b.ToTable("Passwords", "AuthDb");
                });

            modelBuilder.Entity("AuthDb.Server", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ExpireAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.HasKey("Name");

                    b.ToTable("Servers", "AuthDb");
                });

            modelBuilder.Entity("AuthDb.ServerRole", b =>
                {
                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.HasKey("Token");

                    b.ToTable("ServerRoles", "AuthDb");
                });

            modelBuilder.Entity("AuthDb.Account", b =>
                {
                    b.HasOne("AuthDb.Password", null)
                        .WithOne()
                        .HasForeignKey("AuthDb.Account", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
