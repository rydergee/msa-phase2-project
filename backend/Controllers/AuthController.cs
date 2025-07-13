using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MockMate.Api.Models.Auth;
using MockMate.Api.Services.Interfaces;
using MockMate.Api.Services;

namespace MockMate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Register a new user account
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authService.RegisterAsync(request);
            
            _logger.LogInformation("User registered successfully: {Email}", request.Email);
            
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Registration failed: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during registration");
            return StatusCode(500, new { message = "An error occurred during registration" });
        }
    }

    /// <summary>
    /// Login with email and password
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authService.LoginAsync(request);
            
            _logger.LogInformation("User logged in successfully: {Email}", request.Email);
            
            return Ok(response);
        }
        catch (MockMateUnauthorizedAccessException ex)
        {
            _logger.LogWarning("Login failed: {Message}", ex.Message);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during login");
            return StatusCode(500, new { message = "An error occurred during login" });
        }
    }

    /// <summary>
    /// Get current user profile (requires authentication)
    /// </summary>
    [HttpGet("profile")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetProfile()
    {
        try
        {
            var userId = GetCurrentUserId();
            var userProfile = await _authService.GetUserProfileAsync(userId);
            
            return Ok(userProfile);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning("Profile not found: {Message}", ex.Message);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error getting profile");
            return StatusCode(500, new { message = "An error occurred while retrieving profile" });
        }
    }

    /// <summary>
    /// Update user profile (requires authentication)
    /// </summary>
    [HttpPut("profile")]
    [Authorize]
    public async Task<ActionResult<UserDto>> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            var updatedProfile = await _authService.UpdateProfileAsync(userId, request);
            
            _logger.LogInformation("Profile updated for user: {UserId}", userId);
            
            return Ok(updatedProfile);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning("Profile update failed: {Message}", ex.Message);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error updating profile");
            return StatusCode(500, new { message = "An error occurred while updating profile" });
        }
    }

    /// <summary>
    /// Change user password (requires authentication)
    /// </summary>
    [HttpPost("change-password")]
    [Authorize]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            await _authService.ChangePasswordAsync(userId, request);
            
            _logger.LogInformation("Password changed for user: {UserId}", userId);
            
            return Ok(new { message = "Password changed successfully" });
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning("Password change failed: {Message}", ex.Message);
            return NotFound(new { message = ex.Message });
        }
        catch (MockMateUnauthorizedAccessException ex)
        {
            _logger.LogWarning("Password change unauthorized: {Message}", ex.Message);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error changing password");
            return StatusCode(500, new { message = "An error occurred while changing password" });
        }
    }

    /// <summary>
    /// Validate current token (requires authentication)
    /// </summary>
    [HttpGet("validate")]
    [Authorize]
    public async Task<ActionResult> ValidateToken()
    {
        try
        {
            var userId = GetCurrentUserId();
            var isValid = await _authService.ValidateUserAsync(userId);
            
            if (!isValid)
            {
                return Unauthorized(new { message = "Invalid or expired token" });
            }
            
            return Ok(new 
            { 
                message = "Token is valid",
                userId = userId,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error validating token");
            return StatusCode(500, new { message = "An error occurred while validating token" });
        }
    }

    /// <summary>
    /// Logout (client-side token invalidation)
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    public ActionResult Logout()
    {
        try
        {
            var userId = GetCurrentUserId();
            _logger.LogInformation("User logged out: {UserId}", userId);
            
            // JWT tokens are stateless, so logout is primarily client-side
            // In a production app, you might maintain a token blacklist
            return Ok(new { message = "Logged out successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during logout");
            return StatusCode(500, new { message = "An error occurred during logout" });
        }
    }

    /// <summary>
    /// Get current user ID from JWT claims
    /// </summary>
    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            throw new MockMateUnauthorizedAccessException("Invalid user token");
        }
        
        return userId;
    }
}
