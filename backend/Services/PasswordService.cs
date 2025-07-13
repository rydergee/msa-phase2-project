using MockMate.Api.Services.Interfaces;
using BCrypt.Net;

namespace MockMate.Api.Services;

public class PasswordService : IPasswordService
{
    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));
        }
        
        return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        // Handle null or empty inputs gracefully
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(passwordHash))
        {
            return false;
        }
        
        try
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
        catch
        {
            // If verification fails due to invalid hash format or other issues, return false
            return false;
        }
    }
}
