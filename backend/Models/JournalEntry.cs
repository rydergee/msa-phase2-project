using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MockMate.Api.Models;

public class JournalEntry
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(500)]
    public string Question { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Category { get; set; } = string.Empty;

    // STAR Method Components
    [Required]
    [StringLength(2000)]
    public string Situation { get; set; } = string.Empty;

    [Required]
    [StringLength(2000)]
    public string Task { get; set; } = string.Empty;

    [Required]
    [StringLength(2000)]
    public string Action { get; set; } = string.Empty;

    [Required]
    [StringLength(2000)]
    public string Result { get; set; } = string.Empty;

    // Reflection and lessons learned
    [StringLength(1000)]
    public string Reflection { get; set; } = string.Empty;

    // User who owns this entry
    [Required]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Practice tracking
    public int TimesReviewed { get; set; } = 0;
    public DateTime? LastReviewed { get; set; }
}

public enum JournalCategory
{
    Leadership,
    Teamwork,
    ProblemSolving,
    Communication,
    Innovation,
    Conflict,
    Achievement,
    Learning,
    Other
}
