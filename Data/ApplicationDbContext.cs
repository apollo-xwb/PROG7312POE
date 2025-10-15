using Microsoft.EntityFrameworkCore;
using MunicipalServicesApp.DataStructures;

namespace MunicipalServicesApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Issue> Issues { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<SearchHistory> SearchHistories { get; set; }
        public DbSet<UserPreference> UserPreferences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Issue entity
            modelBuilder.Entity<Issue>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Location).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.AttachedFilePath).HasMaxLength(500);
                entity.Property(e => e.SubmittedDate).IsRequired();
            });

            // Configure Event entity
            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Date).IsRequired();
            });

            // Configure SearchHistory entity
            modelBuilder.Entity<SearchHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.SearchTerm).HasMaxLength(200);
                entity.Property(e => e.Category).HasMaxLength(100);
                entity.Property(e => e.SearchDate).IsRequired();
                entity.Property(e => e.ResultCount).IsRequired();
            });

            // Configure UserPreference entity
            modelBuilder.Entity<UserPreference>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PreferenceScore).IsRequired();
                entity.Property(e => e.LastUpdated).IsRequired();
            });
        }
    }
}
