[TestClass]
public class AuthControllerTests
{
    private Mock<IAuthService> _mockAuthService;
    private Mock<ILogger<AuthController>> _mockLogger;
    private AuthController _controller;

    [TestInitialize]
    public void Setup()
    {
        _mockAuthService = new Mock<IAuthService>();
        _mockLogger = new Mock<ILogger<AuthController>>();
        _controller = new AuthController(_mockAuthService.Object, _mockLogger.Object);
    }

    [TestMethod]
    public async Task Register_ValidRequest_ShouldReturnOkWithAuthResponse()
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

        var expectedResponse = new AuthResponse
        {
            User = new UserDto 
            { 
                Id = 1,
                FirstName = "John", 
                LastName = "Doe", 
                Email = "john.doe@example.com" 
            },
            Token = "test-jwt-token"
        };

        _mockAuthService
            .Setup(x => x.RegisterAsync(registerRequest))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Register(registerRequest);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result.Result!;
        okResult.Value.Should().BeOfType<AuthResponse>();
        var authResponse = (AuthResponse)okResult.Value!;
        authResponse.User.Email.Should().Be(registerRequest.Email);
        authResponse.Token.Should().Be("test-jwt-token");

        _mockAuthService.Verify(x => x.RegisterAsync(registerRequest), Times.Once);
    }

    [TestMethod]
    public async Task Register_DuplicateEmail_ShouldReturnBadRequest()
    {
        // Arrange
        var registerRequest = new RegisterRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Password = "password123",
            ConfirmPassword = "password123"
        };

        _mockAuthService
            .Setup(x => x.RegisterAsync(registerRequest))
            .ThrowsAsync(new InvalidOperationException("User with this email already exists"));

        // Act
        var result = await _controller.Register(registerRequest);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = (BadRequestObjectResult)result.Result!;
        badRequestResult.Value.Should().BeEquivalentTo(new { message = "User with this email already exists" });
    }

    [TestMethod]
    public async Task Register_UnexpectedException_ShouldReturnInternalServerError()
    {
        // Arrange
        var registerRequest = new RegisterRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Password = "password123",
            ConfirmPassword = "password123"
        };

        _mockAuthService
            .Setup(x => x.RegisterAsync(registerRequest))
            .ThrowsAsync(new Exception("Database connection error"));

        // Act
        var result = await _controller.Register(registerRequest);

        // Assert
        result.Result.Should().BeOfType<ObjectResult>();
        var statusCodeResult = (ObjectResult)result.Result!;
        statusCodeResult.StatusCode.Should().Be(500);
        statusCodeResult.Value.Should().BeEquivalentTo(new { message = "An error occurred during registration" });
    }

    [TestMethod]
    public async Task Login_ValidCredentials_ShouldReturnOkWithAuthResponse()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = "john.doe@example.com",
            Password = "password123"
        };

        var expectedResponse = new AuthResponse
        {
            User = new UserDto 
            { 
                Id = 1,
                FirstName = "John", 
                LastName = "Doe", 
                Email = "john.doe@example.com" 
            },
            Token = "test-jwt-token"
        };

        _mockAuthService
            .Setup(x => x.LoginAsync(loginRequest))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Login(loginRequest);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result.Result!;
        okResult.Value.Should().BeOfType<AuthResponse>();
        var authResponse = (AuthResponse)okResult.Value!;
        authResponse.User.Email.Should().Be(loginRequest.Email);
        authResponse.Token.Should().Be("test-jwt-token");

        _mockAuthService.Verify(x => x.LoginAsync(loginRequest), Times.Once);
    }

    [TestMethod]
    public async Task Login_InvalidCredentials_ShouldReturnUnauthorized()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = "john.doe@example.com",
            Password = "wrongpassword"
        };

        _mockAuthService
            .Setup(x => x.LoginAsync(loginRequest))
            .ThrowsAsync(new MockMateUnauthorizedAccessException("Invalid email or password"));

        // Act
        var result = await _controller.Login(loginRequest);

        // Assert
        result.Result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = (UnauthorizedObjectResult)result.Result!;
        unauthorizedResult.Value.Should().BeEquivalentTo(new { message = "Invalid email or password" });
    }

    [TestMethod]
    public async Task Login_UnexpectedException_ShouldReturnInternalServerError()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = "john.doe@example.com",
            Password = "password123"
        };

        _mockAuthService
            .Setup(x => x.LoginAsync(loginRequest))
            .ThrowsAsync(new Exception("Database connection error"));

        // Act
        var result = await _controller.Login(loginRequest);

        // Assert
        result.Result.Should().BeOfType<ObjectResult>();
        var statusCodeResult = (ObjectResult)result.Result!;
        statusCodeResult.StatusCode.Should().Be(500);
        statusCodeResult.Value.Should().BeEquivalentTo(new { message = "An error occurred during login" });
    }

    [TestMethod]
    public async Task GetProfile_AuthenticatedUser_ShouldReturnUserProfile()
    {
        // Arrange
        var userId = 1;
        var expectedUserDto = new UserDto
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            University = "Test University",
            StudyField = "Computer Science",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        // Mock the User.Identity.Name to return the user ID
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = principal
            }
        };

        _mockAuthService
            .Setup(x => x.GetUserProfileAsync(userId))
            .ReturnsAsync(expectedUserDto);

        // Act
        var result = await _controller.GetProfile();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result.Result!;
        okResult.Value.Should().BeOfType<UserDto>();
        var userResponse = (UserDto)okResult.Value!;
        userResponse.Id.Should().Be(userId);
        userResponse.Email.Should().Be(expectedUserDto.Email);
        userResponse.FirstName.Should().Be(expectedUserDto.FirstName);
        userResponse.LastName.Should().Be(expectedUserDto.LastName);

        _mockAuthService.Verify(x => x.GetUserProfileAsync(userId), Times.Once);
    }

    [TestMethod]
    public async Task GetProfile_UserNotFound_ShouldReturnNotFound()
    {
        // Arrange
        var userId = 999;
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = principal
            }
        };

        _mockAuthService
            .Setup(x => x.GetUserProfileAsync(userId))
            .ThrowsAsync(new NotFoundException("User not found"));

        // Act
        var result = await _controller.GetProfile();

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = (NotFoundObjectResult)result.Result!;
        notFoundResult.Value.Should().BeEquivalentTo(new { message = "User not found" });
    }

    [TestMethod]
    public async Task GetProfile_InvalidUserId_ShouldReturnInternalServerError()
    {
        // Arrange - No user ID in claims
        var claims = new List<Claim>();
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = principal
            }
        };

        // Act
        var result = await _controller.GetProfile();

        // Assert
        result.Result.Should().BeOfType<ObjectResult>();
        var statusCodeResult = (ObjectResult)result.Result!;
        statusCodeResult.StatusCode.Should().Be(500);
        statusCodeResult.Value.Should().BeEquivalentTo(new { message = "An error occurred while retrieving profile" });
    }
}
