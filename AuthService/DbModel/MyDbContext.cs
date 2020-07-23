﻿using System;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace AuthService.DbModel
{
    public class MyDbContext : DbContext
    {
        private readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public DbSet<UserDb> Users { get; set; }
        public DbSet<SessionDb> Sessions { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSnakeCaseNamingConvention();
            if (Environment.GetEnvironmentVariable("db_host") != null)
            {
                var connectionString = $"Host={Environment.GetEnvironmentVariable("db_host")};Database={Environment.GetEnvironmentVariable("db_name")};Username={Environment.GetEnvironmentVariable("db_username")};Password={Environment.GetEnvironmentVariable("db_password")}";
                optionsBuilder.UseNpgsql(connectionString);
            }
            else
            {
                optionsBuilder.UseNpgsql(
                    $"Host=localhost;Database=my_db;Username=admin;Password=q");

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserDb>().HasIndex(b => b.Username).IsUnique();
        }

        public override int SaveChanges()
        {
            try
            {
                SetVersioning();
                this.BulkSaveChanges();
            }
           
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                throw;
            }
            return 0;
        }

        private void SetVersioning()
        {
            var currentDate = DateTime.UtcNow;
            var entities = this.ChangeTracker.Entries<BaseEntity>();
            foreach (var entityEntry in entities)
            {
                if(entityEntry.State == EntityState.Added)
                    entityEntry.Entity.CreatedAt = currentDate;
            }

        }
    }
}