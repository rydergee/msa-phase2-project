using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockMate.Api.Data;
using MockMate.Api.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace MockMate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InterviewSessionsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<InterviewSessionsController> _logger;

    public InterviewSessionsController(AppDbContext context, ILogger<InterviewSessionsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all interview sessions for the authenticated user
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InterviewSessionDto>>> GetUserSessions()
    {
        try
        {
            var userId = GetCurrentUserId();
            var sessions = await _context.InterviewSessions
                .Include(s => s.Question)
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.CreatedAt)
                .Select(s => MapToDto(s))
                .ToListAsync();

            return Ok(sessions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving interview sessions for user");
            return StatusCode(500, "An error occurred while retrieving interview sessions");
        }
    }

    /// <summary>
    /// Get a specific interview session by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<InterviewSessionDto>> GetSession(int id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var session = await _context.InterviewSessions
                .Include(s => s.Question)
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (session == null)
            {
                return NotFound("Interview session not found");
            }

            return Ok(MapToDto(session));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving interview session {Id}", id);
            return StatusCode(500, "An error occurred while retrieving the interview session");
        }
    }

    /// <summary>
    /// Start a new practice session with a question
    /// </summary>
    [HttpPost("start")]
    public async Task<ActionResult<InterviewSessionDto>> StartSession([FromBody] StartSessionDto startDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            
            // Verify question exists
            var question = await _context.Questions
                .FirstOrDefaultAsync(q => q.Id == startDto.QuestionId && q.IsActive);
            
            if (question == null)
            {
                return NotFound("Question not found");
            }

            var session = new InterviewSession
            {
                UserId = userId,
                QuestionId = startDto.QuestionId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.InterviewSessions.Add(session);
            await _context.SaveChangesAsync();

            // Reload with question for response
            session = await _context.InterviewSessions
                .Include(s => s.Question)
                .FirstAsync(s => s.Id == session.Id);

            return CreatedAtAction(nameof(GetSession), new { id = session.Id }, MapToDto(session));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting interview session");
            return StatusCode(500, "An error occurred while starting the interview session");
        }
    }

    /// <summary>
    /// Submit an answer for a practice session
    /// </summary>
    [HttpPost("{id}/answer")]
    public async Task<ActionResult<InterviewSessionDto>> SubmitAnswer(int id, [FromBody] SubmitAnswerDto answerDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            var session = await _context.InterviewSessions
                .Include(s => s.Question)
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (session == null)
            {
                return NotFound("Interview session not found");
            }

            // Update session with answer
            session.UserAnswer = answerDto.Answer;
            session.ResponseTime = answerDto.ResponseTime;
            session.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(MapToDto(session));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting answer for session {Id}", id);
            return StatusCode(500, "An error occurred while submitting the answer");
        }
    }

    /// <summary>
    /// Rate a practice session
    /// </summary>
    [HttpPost("{id}/rate")]
    public async Task<ActionResult<InterviewSessionDto>> RateSession(int id, [FromBody] RateSessionDto rateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            var session = await _context.InterviewSessions
                .Include(s => s.Question)
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (session == null)
            {
                return NotFound("Interview session not found");
            }

            // Update session with rating and notes
            session.Rating = rateDto.Rating;
            session.Notes = rateDto.Notes;
            session.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(MapToDto(session));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rating session {Id}", id);
            return StatusCode(500, "An error occurred while rating the session");
        }
    }

    /// <summary>
    /// Get user's practice statistics
    /// </summary>
    [HttpGet("stats")]
    public async Task<ActionResult<PracticeStatsDto>> GetPracticeStats()
    {
        try
        {
            var userId = GetCurrentUserId();
            var sessions = await _context.InterviewSessions
                .Include(s => s.Question)
                .Where(s => s.UserId == userId)
                .ToListAsync();

            var totalSessions = sessions.Count;
            var completedSessions = sessions.Count(s => !string.IsNullOrEmpty(s.UserAnswer));
            var averageRating = sessions.Where(s => s.Rating.HasValue).Average(s => s.Rating);
            
            var categoryStats = sessions
                .Where(s => s.Question != null)
                .GroupBy(s => s.Question.Category)
                .Select(g => new CategoryPracticeStatsDto
                {
                    Category = g.Key,
                    SessionCount = g.Count(),
                    CompletedCount = g.Count(s => !string.IsNullOrEmpty(s.UserAnswer)),
                    AverageRating = g.Where(s => s.Rating.HasValue).Average(s => s.Rating) ?? 0
                })
                .OrderByDescending(x => x.SessionCount)
                .ToList();

            var recentSessions = sessions
                .OrderByDescending(s => s.CreatedAt)
                .Take(5)
                .Select(s => new RecentSessionDto
                {
                    Id = s.Id,
                    QuestionText = s.Question?.Text ?? "Unknown Question",
                    Category = s.Question?.Category ?? "Unknown",
                    IsCompleted = !string.IsNullOrEmpty(s.UserAnswer),
                    Rating = s.Rating,
                    CreatedAt = s.CreatedAt
                })
                .ToList();

            var stats = new PracticeStatsDto
            {
                TotalSessions = totalSessions,
                CompletedSessions = completedSessions,
                CompletionRate = totalSessions > 0 ? (double)completedSessions / totalSessions * 100 : 0,
                AverageRating = averageRating ?? 0,
                CategoryBreakdown = categoryStats,
                RecentSessions = recentSessions
            };

            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving practice statistics");
            return StatusCode(500, "An error occurred while retrieving practice statistics");
        }
    }

    /// <summary>
    /// Delete a practice session
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSession(int id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var session = await _context.InterviewSessions
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (session == null)
            {
                return NotFound("Interview session not found");
            }

            _context.InterviewSessions.Remove(session);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting session {Id}", id);
            return StatusCode(500, "An error occurred while deleting the session");
        }
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            throw new UnauthorizedAccessException("User ID not found in claims");
        }
        return userId;
    }

    private static InterviewSessionDto MapToDto(InterviewSession session)
    {
        return new InterviewSessionDto
        {
            Id = session.Id,
            QuestionId = session.QuestionId,
            QuestionText = session.Question?.Text ?? string.Empty,
            QuestionCategory = session.Question?.Category ?? string.Empty,
            QuestionDifficulty = session.Question?.Difficulty ?? string.Empty,
            UserAnswer = session.UserAnswer,
            Rating = session.Rating,
            Notes = session.Notes,
            ResponseTime = session.ResponseTime,
            CreatedAt = session.CreatedAt,
            UpdatedAt = session.UpdatedAt
        };
    }
}

// DTOs for API requests and responses
public class InterviewSessionDto
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string QuestionCategory { get; set; } = string.Empty;
    public string QuestionDifficulty { get; set; } = string.Empty;
    public string? UserAnswer { get; set; }
    public int? Rating { get; set; }
    public string? Notes { get; set; }
    public TimeSpan? ResponseTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class StartSessionDto
{
    [Required]
    public int QuestionId { get; set; }
}

public class SubmitAnswerDto
{
    [Required]
    [MaxLength(2000)]
    public string Answer { get; set; } = string.Empty;
    
    public TimeSpan? ResponseTime { get; set; }
}

public class RateSessionDto
{
    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
}

public class PracticeStatsDto
{
    public int TotalSessions { get; set; }
    public int CompletedSessions { get; set; }
    public double CompletionRate { get; set; }
    public double AverageRating { get; set; }
    public List<CategoryPracticeStatsDto> CategoryBreakdown { get; set; } = new();
    public List<RecentSessionDto> RecentSessions { get; set; } = new();
}

public class CategoryPracticeStatsDto
{
    public string Category { get; set; } = string.Empty;
    public int SessionCount { get; set; }
    public int CompletedCount { get; set; }
    public double AverageRating { get; set; }
}

public class RecentSessionDto
{
    public int Id { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public int? Rating { get; set; }
    public DateTime CreatedAt { get; set; }
}
