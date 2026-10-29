using PaintStore.Model;

namespace PaintStore.API.Interfaces.Services;

public interface IUserService
{
    List<User> GetUsers();
    User? GetUserById(int id);
    List<User> GetUserByName(string name);
    User? GetUserByEmail(string email);

    User AddUser(User user);
    bool UpdateUser(User user);
    bool DeleteUser(int id);
}