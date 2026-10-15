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
}