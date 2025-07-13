using MockMate.Api.Models;

namespace MockMate.Api.Repositories.Interfaces;

public interface IJournalEntryRepository
{
    // Basic CRUD operations
    Task<JournalEntry?> GetByIdAsync(int id);
    Task<JournalEntry?> GetByIdForUserAsync(int id, int userId);
    Task<IEnumerable<JournalEntry>> GetAllForUserAsync(int userId);
    Task<IEnumerable<JournalEntry>> GetByCategoryAsync(int userId, string category);
    
    // Advanced queries
    Task<IEnumerable<JournalEntry>> GetRecentAsync(int userId, int count = 10);
    Task<IEnumerable<JournalEntry>> SearchAsync(int userId, string searchTerm);
    Task<IEnumerable<JournalEntry>> GetMostReviewedAsync(int userId, int count = 5);
    
    // Mutation operations
    Task<JournalEntry> CreateAsync(JournalEntry entry);
    Task<JournalEntry> UpdateAsync(JournalEntry entry);
    Task<bool> DeleteAsync(int id, int userId);
    
    // Practice tracking
    Task IncrementReviewCountAsync(int id, int userId);
    
    // Statistics
    Task<int> GetTotalCountForUserAsync(int userId);
    Task<Dictionary<string, int>> GetCategoryStatsAsync(int userId);
}
