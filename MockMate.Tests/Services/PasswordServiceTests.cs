[TestClass]
public class PasswordServiceTests
{
    private IPasswordService _passwordService;

    [TestInitialize]
    public void Setup()
    {
        _passwordService = new PasswordService();
    }

    [TestMethod]
    public void HashPassword_ShouldReturnHashedPassword()
    {
        // Arrange
        var password = "testPassword123";

        // Act
        var hashedPassword = _passwordService.HashPassword(password);

        // Assert
        hashedPassword.Should().NotBeNullOrEmpty();
        hashedPassword.Should().NotBe(password);
        hashedPassword.Length.Should().BeGreaterThan(50); // BCrypt hashes are typically 60 chars
    }

    [TestMethod]
    public void HashPassword_SamePlaintextPassword_ShouldReturnDifferentHashes()
    {
        // Arrange
        var password = "testPassword123";

        // Act
        var hash1 = _passwordService.HashPassword(password);
        var hash2 = _passwordService.HashPassword(password);

        // Assert
        hash1.Should().NotBe(hash2); // BCrypt uses salt, so hashes should be different
    }

    [TestMethod]
    public void VerifyPassword_CorrectPassword_ShouldReturnTrue()
    {
        // Arrange
        var password = "testPassword123";
        var hashedPassword = _passwordService.HashPassword(password);

        // Act
        var result = _passwordService.VerifyPassword(password, hashedPassword);

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void VerifyPassword_IncorrectPassword_ShouldReturnFalse()
    {
        // Arrange
        var password = "testPassword123";
        var wrongPassword = "wrongPassword";
        var hashedPassword = _passwordService.HashPassword(password);

        // Act
        var result = _passwordService.VerifyPassword(wrongPassword, hashedPassword);

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void VerifyPassword_EmptyPassword_ShouldReturnFalse()
    {
        // Arrange
        var password = "testPassword123";
        var hashedPassword = _passwordService.HashPassword(password);

        // Act
        var result = _passwordService.VerifyPassword("", hashedPassword);

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void VerifyPassword_NullPassword_ShouldReturnFalse()
    {
        // Arrange
        var password = "testPassword123";
        var hashedPassword = _passwordService.HashPassword(password);

        // Act
        var result = _passwordService.VerifyPassword(null!, hashedPassword);

        // Assert
        result.Should().BeFalse();
    }
}
