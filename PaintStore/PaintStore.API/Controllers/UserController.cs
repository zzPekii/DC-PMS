using Microsoft.AspNetCore.Mvc;
using PaintStore.API.DTOs.Users;
using PaintStore.API.Interfaces.Services;
using PaintStore.Model;

namespace PaintStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<List<UserResponseDto>> GetUsers()
    {
        _logger.LogInformation("Getting all users");

        List<UserResponseDto> users = _userService.GetUsers()
            .Select(u => ToResponseDto(u))
            .ToList();

        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public ActionResult<UserResponseDto> GetUserById(int id)
    {
        _logger.LogInformation("Getting user with ID {UserId}", id);

        User? user = _userService.GetUserById(id);

        if (user == null)
        {
            _logger.LogWarning("User {UserId} not found", id);
            return NotFound("Can not find the user");
        }

        return Ok(ToResponseDto(user));
    }

    [HttpGet("name/{name}")]
    public ActionResult<List<UserResponseDto>> GetUserByName(string name)
    {
        List<User> users = _userService.GetUserByName(name);

        if (users.Count == 0)
        {
            _logger.LogWarning("No users found with name {UserName}", name);
            return NotFound("No such name user");
        }

        return Ok(users.Select(u => ToResponseDto(u)).ToList());
    }

    [HttpGet("email/{email}")]
    public ActionResult<UserResponseDto> GetUserByEmail(string email)
    {
        User? user = _userService.GetUserByEmail(email);

        if (user == null)
        {
            _logger.LogWarning("User with email {Email} not found", email);
            return NotFound("Can not find the user");
        }

        return Ok(ToResponseDto(user));
    }

    [HttpPost]
    public ActionResult<UserResponseDto> CreateUser(CreateUserDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name) ||
            string.IsNullOrWhiteSpace(dto.Email))
        {
            _logger.LogWarning("User creation failed: missing name or email");
            return BadRequest("Name and Email cannot be empty");
        }

        if (_userService.GetUserByEmail(dto.Email) != null)
        {
            _logger.LogWarning("User creation failed: duplicate email");
            return BadRequest("Email already exists");
        }

        User user = new User(0, dto.Name)
        {
            Email = dto.Email,
            Phone = dto.Phone
        };

        User createdUser = _userService.AddUser(user);

        _logger.LogInformation(
            "User {UserId} created successfully",
            createdUser.Id);

        return StatusCode(201, ToResponseDto(createdUser));
    }

    [HttpPut("{id:int}")]
    public ActionResult<UserResponseDto> UpdateUser(int id, UpdateUserDto dto)
    {
        User? existingUser = _userService.GetUserById(id);

        if (existingUser == null)
        {
            _logger.LogWarning("User {UserId} not found for update", id);
            return NotFound("Can not find the user");
        }

        if (string.IsNullOrWhiteSpace(dto.Name) ||
            string.IsNullOrWhiteSpace(dto.Email))
        {
            _logger.LogWarning(
                "User {UserId} update failed: invalid input",
                id);

            return BadRequest("Name and Email cannot be empty");
        }

        User? emailOwner = _userService.GetUserByEmail(dto.Email);

        if (emailOwner != null && emailOwner.Id != id)
        {
            _logger.LogWarning(
                "User {UserId} update failed: duplicate email",
                id);

            return BadRequest("Email already exists");
        }

        User user = new User(id, dto.Name)
        {
            Email = dto.Email,
            Phone = dto.Phone
        };

        bool updated = _userService.UpdateUser(user);

        if (!updated)
        {
            _logger.LogWarning("User {UserId} not found for update", id);
            return NotFound("Can not find the user");
        }

        _logger.LogInformation(
            "User {UserId} updated successfully",
            id);

        return Ok(ToResponseDto(existingUser));
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteUser(int id)
    {
        bool deleted = _userService.DeleteUser(id);

        if (!deleted)
        {
            _logger.LogWarning("User {UserId} not found for deletion", id);
            return NotFound("Can not find the user");
        }

        _logger.LogInformation(
            "User {UserId} deleted successfully",
            id);

        return NoContent();
    }

    private static UserResponseDto ToResponseDto(User user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Phone = user.Phone,
            CreatedDate = user.CreatedDate
        };
    }
}
