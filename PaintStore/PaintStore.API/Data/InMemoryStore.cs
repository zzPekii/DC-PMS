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

    public static readonly List<PaintProduct> InitialPaintProducts = new()
    {
        new PaintProduct(
            1, "White Paint", default,
            new PaintSpecification("White", 4),
            new Brand("Dulux"), 50m)
        {
            Stock = 100
        },

        new PaintProduct(
            2, "Black Paint", default,
            new PaintSpecification("Black", 4),
            new Brand("Dulux"), 80m)
        {
            Stock = 80
        },

        new PaintProduct(
            3, "Blue Paint", default,
            new PaintSpecification("Blue", 10),
            new Brand("Taubmans"), 120m)
        {
            Stock = 50
        },

        new PaintProduct(
            4, "Red Paint", default,
            new PaintSpecification("Red", 10),
            new Brand("Taubmans"), 200m)
        {
            Stock = 30
        }
    };

    public List<PaintProduct> PaintProducts { get; } = InitialPaintProducts;

    public List<Order> Orders { get; } = new();
}