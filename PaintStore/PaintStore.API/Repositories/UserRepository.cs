
using PaintStore.API.Data;
using PaintStore.API.Interfaces.Repositories;
using PaintStore.Model;

namespace PaintStore.API.Repositories;

public class UserRepository : IUserRepository
{
    private readonly InMemoryStore _store;

    public UserRepository(InMemoryStore store)
    {
        _store = store;
    }

    public List<User> GetUsers()
    {
        return _store.Users;
    }

    public User? GetUserById(int id)
    {
        return _store.Users.FirstOrDefault(u => u.Id == id);
    }

    public List<User> GetUserByName(string name)
    {
        return _store.Users.Where(u => u.Name == name).ToList();
    }

    public User? GetUserByEmail(string email)
    {
        return _store.Users.FirstOrDefault(
            u => string.Equals(
                u.Email,
                email,
                StringComparison.OrdinalIgnoreCase
            )
        );
    }

    public User AddUser(User user)
    {
        int nextId = _store.Users.Count == 0
            ? 1
            : _store.Users.Max(u => u.Id) + 1;

        user.SetId(nextId);
        _store.Users.Add(user);

        return user;
    }

    public bool UpdateUser(User user)
    {
        User? existingUser = GetUserById(user.Id);

        if (existingUser == null)
        {
            return false;
        }

        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        existingUser.Phone = user.Phone;

        return true;
    }

    public bool DeleteUser(int id)
    {
        User? user = GetUserById(id);

        if (user == null)
        {
            return false;
        }

        return _store.Users.Remove(user);
    }
}
