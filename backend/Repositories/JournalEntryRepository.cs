using Microsoft.EntityFrameworkCore;
using MockMate.Api.Data;
using MockMate.Api.Models;
using MockMate.Api.Repositories.Interfaces;

namespace MockMate.Api.Repositories;

public class JournalEntryRepository : IJournalEntryRepository
{
    private readonly AppDbContext _context;

    public JournalEntryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<JournalEntry?> GetByIdAsync(int id)
    {
        return await _context.JournalEntries
            .Include(j => j.User)
            .FirstOrDefaultAsync(j => j.Id == id);
    }

    public async Task<JournalEntry?> GetByIdForUserAsync(int id, int userId)
    {
        return await _context.JournalEntries
            .Include(j => j.User)
            .FirstOrDefaultAsync(j => j.Id == id && j.UserId == userId);
    }

    public async Task<IEnumerable<JournalEntry>> GetAllForUserAsync(int userId)
    {
        return await _context.JournalEntries
            .Where(j => j.UserId == userId)
            .OrderByDescending(j => j.UpdatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<JournalEntry>> GetByCategoryAsync(int userId, string category)
    {
        return await _context.JournalEntries
            .Where(j => j.UserId == userId && j.Category == category)
            .OrderByDescending(j => j.UpdatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<JournalEntry>> GetByTagsAsync(int userId, string[] tags)
    {
        var query = _context.JournalEntries
            .Where(j => j.UserId == userId);

        foreach (var tag in tags)
        {
            query = query.Where(j => j.Tags.Contains(tag));
        }

        return await query
            .OrderByDescending(j => j.UpdatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<JournalEntry>> GetRecentAsync(int userId, int count = 10)
    {
        return await _context.JournalEntries
            .Where(j => j.UserId == userId)
            .OrderByDescending(j => j.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<JournalEntry>> SearchAsync(int userId, string searchTerm)
    {
        var term = searchTerm.ToLower();
        
        return await _context.JournalEntries
            .Where(j => j.UserId == userId && 
                       (j.Title.ToLower().Contains(term) ||
                        j.Situation.ToLower().Contains(term) ||
                        j.Task.ToLower().Contains(term) ||
                        j.Action.ToLower().Contains(term) ||
                        j.Result.ToLower().Contains(term) ||
                        j.Skills.ToLower().Contains(term) ||
                        j.Tags.ToLower().Contains(term)))
            .OrderByDescending(j => j.UpdatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<JournalEntry>> GetMostReviewedAsync(int userId, int count = 5)
    {
        return await _context.JournalEntries
            .Where(j => j.UserId == userId)
            .OrderByDescending(j => j.TimesReviewed)
            .ThenByDescending(j => j.LastReviewed)
            .Take(count)
            .ToListAsync();
    }

    public async Task<JournalEntry> CreateAsync(JournalEntry entry)
    {
        entry.CreatedAt = DateTime.UtcNow;
        entry.UpdatedAt = DateTime.UtcNow;
        
        _context.JournalEntries.Add(entry);
        await _context.SaveChangesAsync();
        
        return entry;
    }

    public async Task<JournalEntry> UpdateAsync(JournalEntry entry)
    {
        entry.UpdatedAt = DateTime.UtcNow;
        
        _context.JournalEntries.Update(entry);
        await _context.SaveChangesAsync();
        
        return entry;
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        var entry = await GetByIdForUserAsync(id, userId);
        if (entry == null)
            return false;

        _context.JournalEntries.Remove(entry);
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task IncrementReviewCountAsync(int id, int userId)
    {
        var entry = await GetByIdForUserAsync(id, userId);
        if (entry != null)
        {
            entry.TimesReviewed++;
            entry.LastReviewed = DateTime.UtcNow;
            entry.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
        }
    }

    public async Task<int> GetTotalCountForUserAsync(int userId)
    {
        return await _context.JournalEntries
            .CountAsync(j => j.UserId == userId);
    }

    public async Task<Dictionary<string, int>> GetCategoryStatsAsync(int userId)
    {
        return await _context.JournalEntries
            .Where(j => j.UserId == userId)
            .GroupBy(j => j.Category)
            .Select(g => new { Category = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Category, x => x.Count);
    }
}
