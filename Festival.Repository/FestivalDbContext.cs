using System;
using Festival.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Festival.Repository
{
    /// <summary>
    /// The Entity Framework Core Database Context for the Festival Application.
    /// Manages the database connection and the Object-Relational Mapping (ORM) configuration.
    /// </summary>
    public class FestivalDbContext : DbContext
    {
        // DataSets mapping to the database tables
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Show> Shows { get; set; }

        private readonly string _connectionString = "Data Source=festival.db";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Fulfills Requirement 5: Log SQL statements to the console
            optionsBuilder.UseSqlite(_connectionString)
                .LogTo(Console.WriteLine, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Explicitly map entities to the existing table names to avoid pluralization mismatches
            modelBuilder.Entity<Employee>().ToTable("employees");
            modelBuilder.Entity<Show>().ToTable("shows");
            
            // Note: EF Core automatically maps the 'Id' property as the Primary Key.
        }
    }
}