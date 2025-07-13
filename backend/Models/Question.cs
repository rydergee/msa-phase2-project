using System.ComponentModel.DataAnnotations;

namespace MockMate.Api.Models;

public class Question
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(500)]
    public string Text { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(20)]
    public string Difficulty { get; set; } = string.Empty; // Beginner, Intermediate, Advanced
    
    [MaxLength(1000)]
    public string? SampleAnswer { get; set; }
    
    [MaxLength(500)]
    public string? Tips { get; set; }
    
    [MaxLength(200)]
    public string Tags { get; set; } = string.Empty; // Comma-separated tags
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public ICollection<InterviewSession> InterviewSessions { get; set; } = new List<InterviewSession>();
}

public class QuestionCategory
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string? Description { get; set; }
    
    [MaxLength(7)]
    public string Color { get; set; } = "#3B82F6"; // Default blue color
    
    public int SortOrder { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public ICollection<Question> Questions { get; set; } = new List<Question>();
}

public class InterviewSession
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;
    
    [MaxLength(2000)]
    public string? UserAnswer { get; set; }
    
    [Range(1, 5)]
    public int? Rating { get; set; } // 1-5 star rating
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
    
    public TimeSpan? ResponseTime { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
