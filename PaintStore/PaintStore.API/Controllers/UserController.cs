using Microsoft.AspNetCore.Mvc;
using PaintStore.Model;

namespace PaintStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    // mock data
    private static readonly List<User> Users = new()
    {
        new User(1, "Tom")
        {
            Email = "tom@gmail.com",
            Phone = "0411111111"
        },

        new User(2, "Jack")
        {
            Email = "jack@gmail.com",
            Phone = "0422222222"
        },

        new User(3, "Alice")
        {
            Email = "alice@gmail.com",
            Phone = "0433333333"
        }
    };

    [HttpGet]
    public ActionResult<List<User>> GetUsers()
    {
        System.Console.WriteLine("HTTP [GET] 200 OK /api/Users");
        return Ok(Users);
    }

    [HttpGet("{id:int}")]
    public ActionResult<User> GetUserById(int id)
    {
        User user = Users.FirstOrDefault(p => p.Id == id);
        if (user == null)
        {
            // 404 not found
            System.Console.WriteLine($"HTTP [GET] 404 NOT Found /api/Users/{id}");
            return NotFound("Can not find the user");
        }
        System.Console.WriteLine($"HTTP [GET] 200 OK /api/Users/{id}");
        return Ok(user);
    }

    [HttpGet("name/{name}")]
    public ActionResult<List<User>> GetUserByName(string name)
    {
        List<User> users = Users.Where(u => u.Name == name).ToList();
        if (users.Count == 0)
        {
            System.Console.WriteLine($"HTTP [GET] 404 NOT Found /api/Users/{name}");
            return NotFound("No such name user");
        }
        System.Console.WriteLine($"HTTP [GET] 200 OK /api/User/{name}");
        return Ok(users);
    }

    [HttpGet("email/{email}")]
    public ActionResult<User> GetUserByEmail(string email)
    {
        User user = Users.FirstOrDefault(p => p.Email == email);
        if (user == null)
        {
            // 404 not found
            System.Console.WriteLine($"HTTP [GET] 404 NOT Found /api/Users/email/{email}");
            return NotFound("Can not find the user");
        }
        System.Console.WriteLine($"HTTP [GET] 200 OK /api/Users/email/{email}");
        return Ok(user);
    }
}