using System.ComponentModel.DataAnnotations;

namespace MockMate.Api.Models;

public class User
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string? University { get; set; }
    
    [MaxLength(100)]
    public string? StudyField { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties will be added in later tasks
    // public ICollection<JournalEntry> JournalEntries { get; set; } = new List<JournalEntry>();
    // public ICollection<Session> CreatedSessions { get; set; } = new List<Session>();
    // public ICollection<SessionParticipant> SessionParticipations { get; set; } = new List<SessionParticipant>();
}
