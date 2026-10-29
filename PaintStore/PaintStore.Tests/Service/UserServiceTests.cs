using Xunit;
using Moq;
using FluentAssertions;
using PaintStore.API.Interfaces.Repositories;
using PaintStore.API.Services;
using PaintStore.Model;

namespace PaintStore.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockRepository = new Mock<IUserRepository>();
        _userService = new UserService(_mockRepository.Object);
    }

    [Fact]
    public void GetUserById_ExistingUser_ReturnsUser()
    {
        // Arrange
        User user = new User(1, "Tom");

        _mockRepository
            .Setup(r => r.GetUserById(1))
            .Returns(user);

        // Act
        User? result = _userService.GetUserById(1);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Tom");
    }

    [Fact]
    public void GetUserById_NonExistingUser_ReturnsNull()
    {
        // Arrange
        _mockRepository
            .Setup(r => r.GetUserById(999))
            .Returns((User?)null);

        // Act
        User? result = _userService.GetUserById(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void AddUser_ValidUser_ReturnsCreatedUser()
    {
        // Arrange
        User user = new User(0, "David")
        {
            Email = "david@gmail.com"
        };

        _mockRepository
            .Setup(r => r.GetUserByEmail(user.Email))
            .Returns((User?)null);

        _mockRepository
            .Setup(r => r.AddUser(user))
            .Returns(user);

        // Act
        User result = _userService.AddUser(user);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("David");

        _mockRepository.Verify(
            r => r.AddUser(user), Times.Once);
    }

    [Fact]
    public void AddUser_EmptyName_ThrowsArgumentException()
    {
        // Arrange
        User user = new User(0, "")
        {
            Email = "david@gmail.com"
        };

        // Act
        Action act = () => _userService.AddUser(user);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("User name cannot be empty");

        _mockRepository.Verify(
            r => r.AddUser(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public void AddUser_InvalidEmail_ThrowsArgumentException()
    {
        // Arrange
        User user = new User(0, "David")
        {
            Email = "invalid-email"
        };

        // Act
        Action act = () => _userService.AddUser(user);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid email format");
    }

    [Fact]
    public void AddUser_DuplicateEmail_ThrowsArgumentException()
    {
        // Arrange
        User existingUser = new User(1, "Tom")
        {
            Email = "tom@gmail.com"
        };

        User newUser = new User(0, "David")
        {
            Email = "tom@gmail.com"
        };

        _mockRepository
            .Setup(r => r.GetUserByEmail("tom@gmail.com"))
            .Returns(existingUser);

        // Act
        Action act = () => _userService.AddUser(newUser);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Email already exists");
    }

    [Fact]
    public void UpdateUser_ExistingUser_ReturnsTrue()
    {
        // Arrange
        User existingUser = new User(1, "Tom")
        {
            Email = "tom@gmail.com"
        };

        User updatedUser = new User(1, "Tom Smith")
        {
            Email = "tomsmith@gmail.com"
        };

        _mockRepository
            .Setup(r => r.GetUserById(1))
            .Returns(existingUser);

        _mockRepository
            .Setup(r => r.UpdateUser(updatedUser))
            .Returns(true);

        // Act
        bool result = _userService.UpdateUser(updatedUser);

        // Assert
        result.Should().BeTrue();

        _mockRepository.Verify(
            r => r.UpdateUser(updatedUser), Times.Once);
    }

    [Fact]
    public void UpdateUser_NonExistingUser_ReturnsFalse()
    {
        // Arrange
        User user = new User(999, "Nobody")
        {
            Email = "nobody@gmail.com"
        };

        _mockRepository
            .Setup(r => r.GetUserById(999))
            .Returns((User?)null);

        // Act
        bool result = _userService.UpdateUser(user);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void UpdateUser_DuplicateEmail_ThrowsArgumentException()
    {
        // Arrange
        User existingUser = new User(1, "Tom");

        User anotherUser = new User(2, "Jack")
        {
            Email = "jack@gmail.com"
        };

        User updatedUser = new User(1, "Tom")
        {
            Email = "jack@gmail.com"
        };

        _mockRepository
            .Setup(r => r.GetUserById(1))
            .Returns(existingUser);

        _mockRepository
            .Setup(r => r.GetUserByEmail("jack@gmail.com"))
            .Returns(anotherUser);

        // Act
        Action act = () => _userService.UpdateUser(updatedUser);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Email already exists");
    }

    [Fact]
    public void DeleteUser_ExistingUser_ReturnsTrue()
    {
        // Arrange
        _mockRepository
            .Setup(r => r.DeleteUser(1))
            .Returns(true);

        // Act
        bool result = _userService.DeleteUser(1);

        // Assert
        result.Should().BeTrue();

        _mockRepository.Verify(
            r => r.DeleteUser(1), Times.Once);
    }
}
