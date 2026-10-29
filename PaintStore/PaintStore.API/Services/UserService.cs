
using System.ComponentModel.DataAnnotations;
using PaintStore.API.Interfaces.Repositories;
using PaintStore.API.Interfaces.Services;
using PaintStore.Model;

namespace PaintStore.API.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public List<User> GetUsers()
    {
        return _userRepository.GetUsers();
    }

    public User? GetUserById(int id)
    {
        return _userRepository.GetUserById(id);
    }

    public List<User> GetUserByName(string name)
    {
        return _userRepository.GetUserByName(name);
    }

    public User? GetUserByEmail(string email)
    {
        return _userRepository.GetUserByEmail(email);
    }

    public User AddUser(User user)
    {
        // Validate name
        if (string.IsNullOrWhiteSpace(user.Name))
        {
            throw new ArgumentException("User name cannot be empty");
        }

        // Validate email
        if (string.IsNullOrWhiteSpace(user.Email))
        {
            throw new ArgumentException("Email cannot be empty");
        }

        var emailValidator = new EmailAddressAttribute();

        if (!emailValidator.IsValid(user.Email))
        {
            throw new ArgumentException("Invalid email format");
        }

        // Check duplicate email
        User? existingUser = _userRepository.GetUserByEmail(user.Email);

        if (existingUser != null)
        {
            throw new ArgumentException("Email already exists");
        }

        return _userRepository.AddUser(user);
    }

    public bool UpdateUser(User user)
    {
        // Check whether user exists
        User? existingUser = _userRepository.GetUserById(user.Id);

        if (existingUser == null)
        {
            return false;
        }

        // Validate name
        if (string.IsNullOrWhiteSpace(user.Name))
        {
            throw new ArgumentException("User name cannot be empty");
        }

        // Validate email
        if (string.IsNullOrWhiteSpace(user.Email))
        {
            throw new ArgumentException("Email cannot be empty");
        }

        var emailValidator = new EmailAddressAttribute();

        if (!emailValidator.IsValid(user.Email))
        {
            throw new ArgumentException("Invalid email format");
        }

        // Check duplicate email
        User? emailOwner = _userRepository.GetUserByEmail(user.Email);

        if (emailOwner != null && emailOwner.Id != user.Id)
        {
            throw new ArgumentException("Email already exists");
        }

        return _userRepository.UpdateUser(user);
    }

    public bool DeleteUser(int id)
    {
        return _userRepository.DeleteUser(id);
    }
}
