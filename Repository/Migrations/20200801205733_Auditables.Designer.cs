﻿// <auto-generated />

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Repository.DbModel;

namespace Repository.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20200801205733_Auditables")]
    partial class Auditables
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("AuthService.DbModel.SessionDb", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("Active")
                        .HasColumnName("active")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnName("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnName("modified_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("integer");

                    b.HasKey("Id")
                        .HasName("pk_sessions");

                    b.ToTable("sessions");
                });

            modelBuilder.Entity("AuthService.DbModel.UserDb", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnName("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnName("modified_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("ModifiedBy")
                        .HasColumnName("modified_by")
                        .HasColumnType("integer");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnName("password_hash")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnName("password_salt")
                        .HasColumnType("character varying(36)")
                        .HasMaxLength(36);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnName("username")
                        .HasColumnType("character varying(64)")
                        .HasMaxLength(64);

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasName("ix_users_username");

                    b.ToTable("users");
                });
#pragma warning restore 612, 618
        }
    }
}
