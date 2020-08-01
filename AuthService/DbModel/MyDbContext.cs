using System;
using AuthService.Security;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace AuthService.DbModel
{
    public class MyDbContext : DbContext
    {
        private readonly SessionDto _sessionDto;
        private readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public MyDbContext(SessionDto sessionDto)
        {
            _sessionDto = sessionDto;
        }

        public MyDbContext()
        {
        }

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
                //test db
                optionsBuilder.UseNpgsql($"Host=localhost;Database=my_db;Username=admin;Password=q");

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
                SetAuditables();
                this.BulkSaveChanges();
            }
           
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                throw;
            }
            return 0;
        }

        private void SetAuditables()
        {
            var currentDate = DateTime.UtcNow;
            var entities = this.ChangeTracker.Entries<BaseEntity>();
            foreach (var entityEntry in entities)
            {
                if(entityEntry.State == EntityState.Added)
                    entityEntry.Entity.CreatedAt = currentDate;
                if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Entity.ModifiedAt = currentDate;
                    entityEntry.Entity.ModifiedBy = _sessionDto?.UserId;
                }
            }

        }
    }
}