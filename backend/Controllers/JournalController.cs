using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MockMate.Api.Models;
using MockMate.Api.Repositories.Interfaces;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace MockMate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class JournalController : ControllerBase
{
    private readonly IJournalEntryRepository _journalRepository;
    private readonly ILogger<JournalController> _logger;

    public JournalController(IJournalEntryRepository journalRepository, ILogger<JournalController> logger)
    {
        _journalRepository = journalRepository;
        _logger = logger;
    }

    /// <summary>
    /// Get all journal entries for the authenticated user
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<JournalEntryDto>>> GetAll()
    {
        try
        {
            var userId = GetCurrentUserId();
            var entries = await _journalRepository.GetAllForUserAsync(userId);
            
            var entryDtos = entries.Select(MapToDto);
            return Ok(entryDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving journal entries for user");
            return StatusCode(500, "An error occurred while retrieving journal entries");
        }
    }

    /// <summary>
    /// Get a specific journal entry by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<JournalEntryDto>> GetById(int id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var entry = await _journalRepository.GetByIdForUserAsync(id, userId);
            
            if (entry == null)
            {
                return NotFound("Journal entry not found");
            }

            // Increment review count
            await _journalRepository.IncrementReviewCountAsync(id, userId);
            
            return Ok(MapToDto(entry));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving journal entry {Id}", id);
            return StatusCode(500, "An error occurred while retrieving the journal entry");
        }
    }

    /// <summary>
    /// Get journal entries by category
    /// </summary>
    [HttpGet("category/{category}")]
    public async Task<ActionResult<IEnumerable<JournalEntryDto>>> GetByCategory(string category)
    {
        try
        {
            var userId = GetCurrentUserId();
            var entries = await _journalRepository.GetByCategoryAsync(userId, category);
            
            var entryDtos = entries.Select(MapToDto);
            return Ok(entryDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving journal entries by category {Category}", category);
            return StatusCode(500, "An error occurred while retrieving journal entries");
        }
    }

    /// <summary>
    /// Get recent journal entries
    /// </summary>
    [HttpGet("recent")]
    public async Task<ActionResult<IEnumerable<JournalEntryDto>>> GetRecent([FromQuery] int count = 5)
    {
        try
        {
            var userId = GetCurrentUserId();
            var entries = await _journalRepository.GetRecentAsync(userId, count);
            
            var entryDtos = entries.Select(MapToDto);
            return Ok(entryDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving recent journal entries");
            return StatusCode(500, "An error occurred while retrieving recent journal entries");
        }
    }

    /// <summary>
    /// Search journal entries
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<JournalEntryDto>>> Search([FromQuery] string query)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query cannot be empty");
            }

            var userId = GetCurrentUserId();
            var entries = await _journalRepository.SearchAsync(userId, query);
            
            var entryDtos = entries.Select(MapToDto);
            return Ok(entryDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching journal entries with query {Query}", query);
            return StatusCode(500, "An error occurred while searching journal entries");
        }
    }

    /// <summary>
    /// Get most reviewed journal entries
    /// </summary>
    [HttpGet("most-reviewed")]
    public async Task<ActionResult<IEnumerable<JournalEntryDto>>> GetMostReviewed([FromQuery] int count = 5)
    {
        try
        {
            var userId = GetCurrentUserId();
            var entries = await _journalRepository.GetMostReviewedAsync(userId, count);
            
            var entryDtos = entries.Select(MapToDto);
            return Ok(entryDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving most reviewed journal entries");
            return StatusCode(500, "An error occurred while retrieving most reviewed journal entries");
        }
    }

    /// <summary>
    /// Get journal statistics
    /// </summary>
    [HttpGet("stats")]
    public async Task<ActionResult<JournalStatsDto>> GetStats()
    {
        try
        {
            var userId = GetCurrentUserId();
            var totalCount = await _journalRepository.GetTotalCountForUserAsync(userId);
            var categoryStats = await _journalRepository.GetCategoryStatsAsync(userId);
            var recentEntries = await _journalRepository.GetRecentAsync(userId, 3);
            
            var stats = new JournalStatsDto
            {
                TotalEntries = totalCount,
                CategoryBreakdown = categoryStats,
                RecentEntries = recentEntries.Select(MapToSummaryDto).ToList()
            };
            
            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving journal statistics");
            return StatusCode(500, "An error occurred while retrieving journal statistics");
        }
    }

    /// <summary>
    /// Create a new journal entry
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<JournalEntryDto>> Create([FromBody] CreateJournalEntryDto createDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            var entry = new JournalEntry
            {
                UserId = userId,
                Question = createDto.Question,
                Title = createDto.Title,
                Situation = createDto.Situation,
                Task = createDto.Task,
                Action = createDto.Action,
                Result = createDto.Result,
                Skills = createDto.Skills,
                Category = createDto.Category,
                Tags = createDto.Tags ?? string.Empty,
                IsPrivate = createDto.IsPrivate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdEntry = await _journalRepository.CreateAsync(entry);
            var entryDto = MapToDto(createdEntry);
            
            return CreatedAtAction(nameof(GetById), new { id = createdEntry.Id }, entryDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating journal entry");
            return StatusCode(500, "An error occurred while creating the journal entry");
        }
    }

    /// <summary>
    /// Update an existing journal entry
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<JournalEntryDto>> Update(int id, [FromBody] UpdateJournalEntryDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            var existingEntry = await _journalRepository.GetByIdForUserAsync(id, userId);
            
            if (existingEntry == null)
            {
                return NotFound("Journal entry not found");
            }

            // Update properties
            existingEntry.Question = updateDto.Question;
            existingEntry.Title = updateDto.Title;
            existingEntry.Situation = updateDto.Situation;
            existingEntry.Task = updateDto.Task;
            existingEntry.Action = updateDto.Action;
            existingEntry.Result = updateDto.Result;
            existingEntry.Skills = updateDto.Skills;
            existingEntry.Category = updateDto.Category;
            existingEntry.Tags = updateDto.Tags ?? string.Empty;
            existingEntry.IsPrivate = updateDto.IsPrivate;

            var updatedEntry = await _journalRepository.UpdateAsync(existingEntry);
            var entryDto = MapToDto(updatedEntry);
            
            return Ok(entryDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating journal entry {Id}", id);
            return StatusCode(500, "An error occurred while updating the journal entry");
        }
    }

    /// <summary>
    /// Delete a journal entry
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var deleted = await _journalRepository.DeleteAsync(id, userId);
            
            if (!deleted)
            {
                return NotFound("Journal entry not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting journal entry {Id}", id);
            return StatusCode(500, "An error occurred while deleting the journal entry");
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

    private static JournalEntryDto MapToDto(JournalEntry entry)
    {
        return new JournalEntryDto
        {
            Id = entry.Id,
            Question = entry.Question,
            Title = entry.Title,
            Situation = entry.Situation,
            Task = entry.Task,
            Action = entry.Action,
            Result = entry.Result,
            Skills = entry.Skills,
            Category = entry.Category,
            Tags = entry.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(t => t.Trim())
                        .Where(t => !string.IsNullOrEmpty(t))
                        .ToList(),
            IsPrivate = entry.IsPrivate,
            TimesReviewed = entry.TimesReviewed,
            LastReviewed = entry.LastReviewed,
            CreatedAt = entry.CreatedAt,
            UpdatedAt = entry.UpdatedAt
        };
    }

    private static JournalEntrySummaryDto MapToSummaryDto(JournalEntry entry)
    {
        return new JournalEntrySummaryDto
        {
            Id = entry.Id,
            Question = entry.Question,
            Title = entry.Title,
            Category = entry.Category,
            CreatedAt = entry.CreatedAt,
            TimesReviewed = entry.TimesReviewed
        };
    }
}

// DTOs for API responses and requests
public class JournalEntryDto
{
    public int Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Situation { get; set; } = string.Empty;
    public string Task { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string Result { get; set; } = string.Empty;
    public string Skills { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public bool IsPrivate { get; set; }
    public int TimesReviewed { get; set; }
    public DateTime? LastReviewed { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class JournalEntrySummaryDto
{
    public int Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int TimesReviewed { get; set; }
}

public class CreateJournalEntryDto
{
    [Required]
    [MaxLength(500)]
    public string Question { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(1000)]
    public string Situation { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(1000)]
    public string Task { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(1500)]
    public string Action { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(1000)]
    public string Result { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string Skills { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string? Tags { get; set; }
    
    public bool IsPrivate { get; set; } = true;
}

public class UpdateJournalEntryDto
{
    [Required]
    [MaxLength(500)]
    public string Question { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(1000)]
    public string Situation { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(1000)]
    public string Task { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(1500)]
    public string Action { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(1000)]
    public string Result { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string Skills { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string? Tags { get; set; }
    
    public bool IsPrivate { get; set; }
}

public class JournalStatsDto
{
    public int TotalEntries { get; set; }
    public Dictionary<string, int> CategoryBreakdown { get; set; } = new();
    public List<JournalEntrySummaryDto> RecentEntries { get; set; } = new();
}
