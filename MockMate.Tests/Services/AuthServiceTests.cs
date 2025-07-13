[TestClass]
public class AuthServiceTests
{
    private AppDbContext _context;
    private AuthService _authService;
    private Mock<IPasswordService> _mockPasswordService;
    private Mock<ILogger<AuthService>> _mockLogger;
    private JwtSettings _jwtSettings;

    [TestInitialize]
    public void Setup()
    {
        // Setup in-memory database
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);

        // Setup mocks and dependencies
        _mockPasswordService = new Mock<IPasswordService>();
        _mockLogger = new Mock<ILogger<AuthService>>();
        _jwtSettings = new JwtSettings
        {
            SecretKey = "ThisIsATestSecretKeyForJWTTokenGenerationThatIsLongEnough123456789",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ExpiryInMinutes = 60
        };

        var jwtOptions = Options.Create(_jwtSettings);
        _authService = new AuthService(_context, _mockPasswordService.Object, jwtOptions, _mockLogger.Object);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Dispose();
    }

    [TestMethod]
    public async Task RegisterAsync_ValidUser_ShouldCreateUserAndReturnResponse()
    {
        // Arrange
        var registerRequest = new RegisterRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Password = "password123",
            ConfirmPassword = "password123",
            University = "Test University",
            StudyField = "Computer Science"
        };

        _mockPasswordService
            .Setup(x => x.HashPassword(registerRequest.Password))
            .Returns("hashedPassword123");

        // Act
        var result = await _authService.RegisterAsync(registerRequest);

        // Assert
        result.Should().NotBeNull();
        result.User.Should().NotBeNull();
        result.User.Email.Should().Be(registerRequest.Email);
        result.User.FirstName.Should().Be(registerRequest.FirstName);
        result.User.LastName.Should().Be(registerRequest.LastName);
        result.Token.Should().NotBeNullOrEmpty();

        // Verify user was saved to database
        var savedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerRequest.Email);
        savedUser.Should().NotBeNull();
        savedUser!.PasswordHash.Should().Be("hashedPassword123");
        
        _mockPasswordService.Verify(x => x.HashPassword(registerRequest.Password), Times.Once);
    }

    [TestMethod]
    public async Task RegisterAsync_DuplicateEmail_ShouldThrowException()
    {
        // Arrange
        var existingUser = new User
        {
            FirstName = "Existing",
            LastName = "User",
            Email = "john.doe@example.com",
            PasswordHash = "existingHash",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };
        _context.Users.Add(existingUser);
        await _context.SaveChangesAsync();

        var registerRequest = new RegisterRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Password = "password123",
            ConfirmPassword = "password123"
        };

        // Act & Assert
        await _authService
            .Invoking(s => s.RegisterAsync(registerRequest))
            .Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("User with this email already exists");
    }

    [TestMethod]
    public async Task LoginAsync_ValidCredentials_ShouldReturnAuthResponse()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PasswordHash = "hashedPassword123",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var loginRequest = new LoginRequest
        {
            Email = "john.doe@example.com",
            Password = "password123"
        };

        _mockPasswordService
            .Setup(x => x.VerifyPassword(loginRequest.Password, user.PasswordHash))
            .Returns(true);

        // Act
        var result = await _authService.LoginAsync(loginRequest);

        // Assert
        result.Should().NotBeNull();
        result.User.Should().NotBeNull();
        result.User.Email.Should().Be(loginRequest.Email);
        result.Token.Should().NotBeNullOrEmpty();
        
        _mockPasswordService.Verify(x => x.VerifyPassword(loginRequest.Password, user.PasswordHash), Times.Once);
    }

    [TestMethod]
    public async Task LoginAsync_InvalidEmail_ShouldThrowException()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = "nonexistent@example.com",
            Password = "password123"
        };

        // Act & Assert
        await _authService
            .Invoking(s => s.LoginAsync(loginRequest))
            .Should()
            .ThrowAsync<MockMateUnauthorizedAccessException>()
            .WithMessage("Invalid email or password");
    }

    [TestMethod]
    public async Task LoginAsync_InvalidPassword_ShouldThrowException()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PasswordHash = "hashedPassword123",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var loginRequest = new LoginRequest
        {
            Email = "john.doe@example.com",
            Password = "wrongPassword"
        };

        _mockPasswordService
            .Setup(x => x.VerifyPassword(loginRequest.Password, user.PasswordHash))
            .Returns(false);

        // Act & Assert
        await _authService
            .Invoking(s => s.LoginAsync(loginRequest))
            .Should()
            .ThrowAsync<MockMateUnauthorizedAccessException>()
            .WithMessage("Invalid email or password");
    }

    [TestMethod]
    public async Task LoginAsync_InactiveUser_ShouldThrowException()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PasswordHash = "hashedPassword123",
            CreatedAt = DateTime.UtcNow,
            IsActive = false // Inactive user
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var loginRequest = new LoginRequest
        {
            Email = "john.doe@example.com",
            Password = "password123"
        };

        // Act & Assert
        await _authService
            .Invoking(s => s.LoginAsync(loginRequest))
            .Should()
            .ThrowAsync<MockMateUnauthorizedAccessException>()
            .WithMessage("Invalid email or password");
    }

    [TestMethod]
    public async Task GetUserProfileAsync_ExistingUser_ShouldReturnUserDto()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PasswordHash = "hashedPassword123",
            University = "Test University",
            StudyField = "Computer Science",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _authService.GetUserProfileAsync(user.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(user.Id);
        result.Email.Should().Be(user.Email);
        result.FirstName.Should().Be(user.FirstName);
        result.LastName.Should().Be(user.LastName);
        result.University.Should().Be(user.University);
        result.StudyField.Should().Be(user.StudyField);
    }

    [TestMethod]
    public async Task GetUserProfileAsync_NonExistentUser_ShouldThrowNotFoundException()
    {
        // Arrange
        var nonExistentId = 999;

        // Act & Assert
        await _authService
            .Invoking(s => s.GetUserProfileAsync(nonExistentId))
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage("User not found");
    }
}
