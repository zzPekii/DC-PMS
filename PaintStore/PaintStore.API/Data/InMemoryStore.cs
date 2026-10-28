using PaintStore.Model;

namespace PaintStore.API.Data;

public class InMemoryStore
{
    public List<User> Users { get; } = new()
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
}