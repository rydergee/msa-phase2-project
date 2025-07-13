namespace MockMate.Api.Models.Configuration;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";
    
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpiryInMinutes { get; set; } = 60; // Default 1 hour
    public int RefreshTokenExpiryInDays { get; set; } = 7; // Default 7 days
}
