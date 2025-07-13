using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockMate.Api.Data;
using MockMate.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace MockMate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<QuestionsController> _logger;

    public QuestionsController(AppDbContext context, ILogger<QuestionsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all active questions with optional filtering
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<QuestionResponseDto>> GetQuestions(
        [FromQuery] string? category = null,
        [FromQuery] string? difficulty = null,
        [FromQuery] string? tags = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        try
        {
            var query = _context.Questions.Where(q => q.IsActive);

            // Apply filters
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(q => q.Category.ToLower() == category.ToLower());
            }

            if (!string.IsNullOrEmpty(difficulty))
            {
                query = query.Where(q => q.Difficulty.ToLower() == difficulty.ToLower());
            }

            if (!string.IsNullOrEmpty(tags))
            {
                var tagList = tags.Split(',').Select(t => t.Trim().ToLower());
                query = query.Where(q => tagList.Any(tag => q.Tags.ToLower().Contains(tag)));
            }

            // Get total count for pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            var questions = await query
                .OrderBy(q => q.Category)
                .ThenBy(q => q.Difficulty)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Text = q.Text,
                    Category = q.Category,
                    Difficulty = q.Difficulty,
                    SampleAnswer = q.SampleAnswer,
                    Tips = q.Tips,
                    Tags = q.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                              .Select(t => t.Trim())
                              .Where(t => !string.IsNullOrEmpty(t))
                              .ToList()
                })
                .ToListAsync();

            var response = new QuestionResponseDto
            {
                Questions = questions,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving questions");
            return StatusCode(500, "An error occurred while retrieving questions");
        }
    }

    /// <summary>
    /// Get a specific question by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<QuestionDto>> GetQuestion(int id)
    {
        try
        {
            var question = await _context.Questions
                .Where(q => q.Id == id && q.IsActive)
                .Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Text = q.Text,
                    Category = q.Category,
                    Difficulty = q.Difficulty,
                    SampleAnswer = q.SampleAnswer,
                    Tips = q.Tips,
                    Tags = q.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                              .Select(t => t.Trim())
                              .Where(t => !string.IsNullOrEmpty(t))
                              .ToList()
                })
                .FirstOrDefaultAsync();

            if (question == null)
            {
                return NotFound("Question not found");
            }

            return Ok(question);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving question {Id}", id);
            return StatusCode(500, "An error occurred while retrieving the question");
        }
    }

    /// <summary>
    /// Search questions by text content
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<QuestionResponseDto>> SearchQuestions(
        [FromQuery] string query,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query cannot be empty");
            }

            var searchQuery = _context.Questions
                .Where(q => q.IsActive && 
                       (q.Text.ToLower().Contains(query.ToLower()) ||
                        q.Category.ToLower().Contains(query.ToLower()) ||
                        q.Tags.ToLower().Contains(query.ToLower())));

            var totalCount = await searchQuery.CountAsync();

            var questions = await searchQuery
                .OrderBy(q => q.Category)
                .ThenBy(q => q.Difficulty)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Text = q.Text,
                    Category = q.Category,
                    Difficulty = q.Difficulty,
                    SampleAnswer = q.SampleAnswer,
                    Tips = q.Tips,
                    Tags = q.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                              .Select(t => t.Trim())
                              .Where(t => !string.IsNullOrEmpty(t))
                              .ToList()
                })
                .ToListAsync();

            var response = new QuestionResponseDto
            {
                Questions = questions,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching questions with query {Query}", query);
            return StatusCode(500, "An error occurred while searching questions");
        }
    }

    /// <summary>
    /// Get random questions for practice
    /// </summary>
    [HttpGet("random")]
    public async Task<ActionResult<IEnumerable<QuestionDto>>> GetRandomQuestions(
        [FromQuery] int count = 5,
        [FromQuery] string? category = null,
        [FromQuery] string? difficulty = null)
    {
        try
        {
            var query = _context.Questions.Where(q => q.IsActive);

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(q => q.Category.ToLower() == category.ToLower());
            }

            if (!string.IsNullOrEmpty(difficulty))
            {
                query = query.Where(q => q.Difficulty.ToLower() == difficulty.ToLower());
            }

            var questions = await query
                .OrderBy(x => Guid.NewGuid()) // Random ordering
                .Take(count)
                .Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Text = q.Text,
                    Category = q.Category,
                    Difficulty = q.Difficulty,
                    SampleAnswer = q.SampleAnswer,
                    Tips = q.Tips,
                    Tags = q.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                              .Select(t => t.Trim())
                              .Where(t => !string.IsNullOrEmpty(t))
                              .ToList()
                })
                .ToListAsync();

            return Ok(questions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving random questions");
            return StatusCode(500, "An error occurred while retrieving random questions");
        }
    }

    /// <summary>
    /// Get all question categories
    /// </summary>
    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<QuestionCategoryDto>>> GetCategories()
    {
        try
        {
            var categories = await _context.QuestionCategories
                .Where(c => c.IsActive)
                .OrderBy(c => c.SortOrder)
                .ThenBy(c => c.Name)
                .Select(c => new QuestionCategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Color = c.Color,
                    QuestionCount = _context.Questions.Count(q => q.Category == c.Name && q.IsActive)
                })
                .ToListAsync();

            return Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving question categories");
            return StatusCode(500, "An error occurred while retrieving categories");
        }
    }

    /// <summary>
    /// Get questions by category
    /// </summary>
    [HttpGet("categories/{categoryName}")]
    public async Task<ActionResult<QuestionResponseDto>> GetQuestionsByCategory(
        string categoryName,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        try
        {
            var query = _context.Questions
                .Where(q => q.IsActive && q.Category.ToLower() == categoryName.ToLower());

            var totalCount = await query.CountAsync();

            var questions = await query
                .OrderBy(q => q.Difficulty)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Text = q.Text,
                    Category = q.Category,
                    Difficulty = q.Difficulty,
                    SampleAnswer = q.SampleAnswer,
                    Tips = q.Tips,
                    Tags = q.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                              .Select(t => t.Trim())
                              .Where(t => !string.IsNullOrEmpty(t))
                              .ToList()
                })
                .ToListAsync();

            var response = new QuestionResponseDto
            {
                Questions = questions,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving questions for category {Category}", categoryName);
            return StatusCode(500, "An error occurred while retrieving questions for this category");
        }
    }

    /// <summary>
    /// Get questions statistics
    /// </summary>
    [HttpGet("stats")]
    public async Task<ActionResult<QuestionStatsDto>> GetQuestionStats()
    {
        try
        {
            var totalQuestions = await _context.Questions.CountAsync(q => q.IsActive);
            
            var categoryStats = await _context.Questions
                .Where(q => q.IsActive)
                .GroupBy(q => q.Category)
                .Select(g => new CategoryStatsDto
                {
                    Category = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            var difficultyStats = await _context.Questions
                .Where(q => q.IsActive)
                .GroupBy(q => q.Difficulty)
                .Select(g => new DifficultyStatsDto
                {
                    Difficulty = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            var stats = new QuestionStatsDto
            {
                TotalQuestions = totalQuestions,
                CategoryBreakdown = categoryStats,
                DifficultyBreakdown = difficultyStats
            };

            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving question statistics");
            return StatusCode(500, "An error occurred while retrieving question statistics");
        }
    }
}

// DTOs for API responses
public class QuestionDto
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
    public string? SampleAnswer { get; set; }
    public string? Tips { get; set; }
    public List<string> Tags { get; set; } = new();
}

public class QuestionResponseDto
{
    public List<QuestionDto> Questions { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}

public class QuestionCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Color { get; set; } = string.Empty;
    public int QuestionCount { get; set; }
}

public class QuestionStatsDto
{
    public int TotalQuestions { get; set; }
    public List<CategoryStatsDto> CategoryBreakdown { get; set; } = new();
    public List<DifficultyStatsDto> DifficultyBreakdown { get; set; } = new();
}

public class CategoryStatsDto
{
    public string Category { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class DifficultyStatsDto
{
    public string Difficulty { get; set; } = string.Empty;
    public int Count { get; set; }
}
