
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

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public ActionResult<List<UserResponseDto>> GetUsers()
    {
        List<UserResponseDto> users = _userService.GetUsers().Select(u => ToResponseDto(u)).ToList();

        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public ActionResult<UserResponseDto> GetUserById(int id)
    {
        User? user = _userService.GetUserById(id);

        if (user == null)
        {
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
            return BadRequest("Name and Email cannot be empty");
        }

        if (_userService.GetUserByEmail(dto.Email) != null)
        {
            return BadRequest("Email already exists");
        }

        User user = new User(0, dto.Name)
        {
            Email = dto.Email,
            Phone = dto.Phone
        };

        User createdUser = _userService.AddUser(user);

        return StatusCode(201, ToResponseDto(createdUser));
    }

    [HttpPut("{id:int}")]
    public ActionResult<UserResponseDto> UpdateUser(int id, UpdateUserDto dto)
    {
        User? existingUser = _userService.GetUserById(id);

        if (existingUser == null)
        {
            return NotFound("Can not find the user");
        }

        if (string.IsNullOrWhiteSpace(dto.Name) ||
            string.IsNullOrWhiteSpace(dto.Email))
        {
            return BadRequest("Name and Email cannot be empty");
        }

        User? emailOwner = _userService.GetUserByEmail(dto.Email);

        if (emailOwner != null && emailOwner.Id != id)
        {
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
            return NotFound("Can not find the user");
        }

        return Ok(ToResponseDto(existingUser));
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteUser(int id)
    {
        bool deleted = _userService.DeleteUser(id);

        if (!deleted)
        {
            return NotFound("Can not find the user");
        }

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
