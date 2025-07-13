using Microsoft.EntityFrameworkCore;
using MockMate.Api.Models;

namespace MockMate.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<JournalEntry> JournalEntries { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionCategory> QuestionCategories { get; set; }
    public DbSet<InterviewSession> InterviewSessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User entity configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.FirstName).IsRequired();
            entity.Property(e => e.LastName).IsRequired();
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("datetime('now')");
        });

        // JournalEntry entity configuration
        modelBuilder.Entity<JournalEntry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Situation).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.Task).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.Action).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.Result).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.Reflection).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("datetime('now')");
            entity.Property(e => e.TimesReviewed).HasDefaultValue(0);

            // Foreign key relationship
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Indexes for performance
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => new { e.UserId, e.Category });
            entity.HasIndex(e => new { e.UserId, e.CreatedAt });
        });

        // Question entity configuration
        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Text).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Difficulty).IsRequired().HasMaxLength(20);
            entity.Property(e => e.SampleAnswer).HasMaxLength(1000);
            entity.Property(e => e.Tips).HasMaxLength(500);
            entity.Property(e => e.Tags).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("datetime('now')");

            // Indexes for performance
            entity.HasIndex(e => e.Category);
            entity.HasIndex(e => e.Difficulty);
            entity.HasIndex(e => e.IsActive);
        });

        // QuestionCategory entity configuration
        modelBuilder.Entity<QuestionCategory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Color).HasMaxLength(7).HasDefaultValue("#3B82F6");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasIndex(e => e.Name).IsUnique();
            entity.HasIndex(e => e.SortOrder);
        });

        // InterviewSession entity configuration
        modelBuilder.Entity<InterviewSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserAnswer).HasMaxLength(2000);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("datetime('now')");

            // Foreign key relationships
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Question)
                  .WithMany(q => q.InterviewSessions)
                  .HasForeignKey(e => e.QuestionId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Indexes for performance
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => new { e.UserId, e.QuestionId });
            entity.HasIndex(e => new { e.UserId, e.CreatedAt });
        });

        // Seed data
        modelBuilder.Entity<QuestionCategory>().HasData(SeedData.Questions.Categories);
        modelBuilder.Entity<Question>().HasData(SeedData.Questions.BehavioralQuestions);

        // Additional entity configurations will be added in future tasks
        // ConfigureJournalEntry(modelBuilder);
        // ConfigureQuestion(modelBuilder);
        // ConfigureSession(modelBuilder);
        // ConfigureFeedback(modelBuilder);
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.Entity is User user)
            {
                user.UpdatedAt = DateTime.UtcNow;
            }
            else if (entry.Entity is JournalEntry journalEntry)
            {
                journalEntry.UpdatedAt = DateTime.UtcNow;
            }
            else if (entry.Entity is Question question)
            {
                question.UpdatedAt = DateTime.UtcNow;
            }
            else if (entry.Entity is InterviewSession session)
            {
                session.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
