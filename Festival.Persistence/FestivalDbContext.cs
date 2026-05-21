using System;
using Festival.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Festival.Persistence
{
    /// <summary>
    /// Entity Framework Core database context for the Festival Application.
    /// Handles database connections, entity mappings, and SQL generation.
    /// </summary>
    public class FestivalDbContext : DbContext
    {
        // DataSets mapping directly to the SQLite database tables
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Show> Shows { get; set; }

        // FIX 1: We must use the simple connection string.
        // The old DbUtils configuration likely contains "Version=3" which is NOT supported by EF Core SQLite.
        private readonly string _connectionString = "Data Source=festival.db";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Fulfills Requirement: Log generated SQL statements to the server console
            optionsBuilder.UseSqlite(_connectionString)
                          .LogTo(Console.WriteLine, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Employee Mapping
            modelBuilder.Entity<Employee>().ToTable("employees");

            // 2. Show Mapping
            modelBuilder.Entity<Show>().ToTable("shows");
            
            // FIX 2: Complete Column Mapping based on the exact database schema
            
            modelBuilder.Entity<Show>()
                .Property(s => s.ArtistId)
                .HasColumnName("artist_id"); 

            // Mapped based on the provided database screenshot
            modelBuilder.Entity<Show>()
                .Property(s => s.Date)
                .HasColumnName("show_date"); 

            // Mapped based on the provided database screenshot
            modelBuilder.Entity<Show>()
                .Property(s => s.Time)
                .HasColumnName("show_time"); 

            modelBuilder.Entity<Show>()
                .Property(s => s.AvailableSeats)
                .HasColumnName("available_seats"); 

            modelBuilder.Entity<Show>()
                .Property(s => s.SoldSeats)
                .HasColumnName("sold_seats"); 
                
            // EF Core automatically identifies the 'Id' property as the Primary Key.
        }
    }
}