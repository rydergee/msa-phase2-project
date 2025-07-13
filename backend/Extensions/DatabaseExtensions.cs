using Microsoft.EntityFrameworkCore;
using MockMate.Api.Data;

namespace MockMate.Api.Extensions;

public static class DatabaseExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        try
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();
            
            // Log database initialization
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();
            logger.LogInformation("Database initialized successfully with SQLite");
            
            // Add any seed data here in future tasks
            await SeedDataAsync(context);
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();
            logger.LogError(ex, "An error occurred while initializing the database");
            throw;
        }
    }
    
    private static async Task SeedDataAsync(AppDbContext context)
    {
        // Check if we need to seed any initial data
        if (!await context.Users.AnyAsync())
        {
            // Seed data will be added in future tasks
            // For now, just ensure the Users table exists
            await context.SaveChangesAsync();
        }
    }
}
