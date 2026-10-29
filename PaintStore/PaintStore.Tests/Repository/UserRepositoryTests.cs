using Xunit;
using PaintStore.API.Data;
using PaintStore.API.Repositories;
using PaintStore.Model;

namespace PaintStore.Tests.Repositories;

public class UserRepositoryTests
{
    [Fact]
    public void GetUserById_ExistingUser_ReturnsUser()
    {
        // Arrange
        InMemoryStore store = new InMemoryStore();
        UserRepository repository = new UserRepository(store);

        // Act
        User? user = repository.GetUserById(1);

        // Assert
        Assert.NotNull(user);
        Assert.Equal(1, user.Id);
        Assert.Equal("Tom", user.Name);
    }

    [Fact]
    public void GetUserById_NonExistingUser_ReturnsNull()
    {
        // Arrange
        InMemoryStore store = new InMemoryStore();
        UserRepository repository = new UserRepository(store);

        // Act
        User? user = repository.GetUserById(999);

        // Assert
        Assert.Null(user);
    }

    [Fact]
    public void AddUser_ValidUser_AddsUser()
    {
        // Arrange
        InMemoryStore store = new InMemoryStore();
        UserRepository repository = new UserRepository(store);

        User newUser = new User(0, "David")
        {
            Email = "david@gmail.com",
            Phone = "0412345678"
        };

        // Act
        User result = repository.AddUser(newUser);

        // Assert
        Assert.Equal(4, result.Id);
        Assert.Equal("David", result.Name);
        Assert.Equal(4, repository.GetUsers().Count);
    }

    [Fact]
    public void DeleteUser_ExistingUser_RemovesUser()
    {
        // Arrange
        InMemoryStore store = new InMemoryStore();
        UserRepository repository = new UserRepository(store);

        // Act
        bool deleted = repository.DeleteUser(1);

        // Assert
        Assert.True(deleted);
        Assert.Null(repository.GetUserById(1));
    }
}
