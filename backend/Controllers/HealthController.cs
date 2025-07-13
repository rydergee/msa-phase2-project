using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockMate.Api.Data;
using System.Reflection;

namespace MockMate.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<HealthController> _logger;

    public HealthController(AppDbContext context, ILogger<HealthController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Health check endpoint for monitoring
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<HealthResponse>> Get()
    {
        try
        {
            var response = new HealthResponse
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Version = GetVersion(),
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown"
            };

            // Check database connectivity
            var canConnectToDatabase = await CheckDatabaseConnection();
            response.Checks.Add("database", canConnectToDatabase ? "Healthy" : "Unhealthy");

            // Check memory usage
            var memoryUsage = GetMemoryUsage();
            response.Checks.Add("memory", memoryUsage < 500 ? "Healthy" : "Warning"); // 500MB threshold
            response.MemoryUsageMB = memoryUsage;

            // Check disk space (if applicable)
            var diskSpace = GetAvailableDiskSpace();
            response.Checks.Add("disk", diskSpace > 1000 ? "Healthy" : "Warning"); // 1GB threshold
            response.AvailableDiskSpaceMB = diskSpace;

            // Overall status
            var hasUnhealthyChecks = response.Checks.Values.Any(status => status == "Unhealthy");
            if (hasUnhealthyChecks)
            {
                response.Status = "Unhealthy";
                return StatusCode(503, response);
            }

            var hasWarnings = response.Checks.Values.Any(status => status == "Warning");
            if (hasWarnings)
            {
                response.Status = "Degraded";
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Health check failed");
            
            var errorResponse = new HealthResponse
            {
                Status = "Unhealthy",
                Timestamp = DateTime.UtcNow,
                Version = GetVersion(),
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
                Error = ex.Message
            };

            return StatusCode(503, errorResponse);
        }
    }

    /// <summary>
    /// Simple liveness probe
    /// </summary>
    [HttpGet("live")]
    public ActionResult GetLiveness()
    {
        return Ok(new { status = "alive", timestamp = DateTime.UtcNow });
    }

    /// <summary>
    /// Readiness probe - checks if app is ready to serve traffic
    /// </summary>
    [HttpGet("ready")]
    public async Task<ActionResult> GetReadiness()
    {
        try
        {
            // Check if database is accessible
            var canConnectToDatabase = await CheckDatabaseConnection();
            if (!canConnectToDatabase)
            {
                return StatusCode(503, new { status = "not ready", reason = "database unavailable" });
            }

            return Ok(new { status = "ready", timestamp = DateTime.UtcNow });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Readiness check failed");
            return StatusCode(503, new { status = "not ready", error = ex.Message });
        }
    }

    private async Task<bool> CheckDatabaseConnection()
    {
        try
        {
            return await _context.Database.CanConnectAsync();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Database connection check failed");
            return false;
        }
    }

    private static string GetVersion()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var version = assembly.GetName().Version;
        return version?.ToString() ?? "Unknown";
    }

    private static long GetMemoryUsage()
    {
        try
        {
            var workingSet = Environment.WorkingSet;
            return workingSet / (1024 * 1024); // Convert to MB
        }
        catch
        {
            return 0;
        }
    }

    private static long GetAvailableDiskSpace()
    {
        try
        {
            var drives = DriveInfo.GetDrives();
            var systemDrive = drives.FirstOrDefault(d => d.IsReady && d.Name == Path.GetPathRoot(Environment.CurrentDirectory));
            if (systemDrive != null)
            {
                return systemDrive.AvailableFreeSpace / (1024 * 1024); // Convert to MB
            }
            return 0;
        }
        catch
        {
            return 0;
        }
    }
}

public class HealthResponse
{
    public string Status { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string Version { get; set; } = string.Empty;
    public string Environment { get; set; } = string.Empty;
    public Dictionary<string, string> Checks { get; set; } = new();
    public long MemoryUsageMB { get; set; }
    public long AvailableDiskSpaceMB { get; set; }
    public string? Error { get; set; }
}
