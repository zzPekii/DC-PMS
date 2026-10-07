namespace DCPMS;

public class Order
{
    public DateTime CreatedAt { get; set; }
    public PaintProduct[] Products { get; set; }

    public Order(PaintProduct[] products)
    {
        CreatedAt = DateTime.Now;
        Products = products;
    }

    public decimal GetTotalOrderPrice()
    {
        decimal total = 0;
        foreach (PaintProduct product in Products)
        {
            total += product.GetFinalPrice();
        }
        return total;
    }

    public void DisplayOrder()
    {
        Console.WriteLine("===== Order =====");
        Console.WriteLine($"Created At: {CreatedAt}");

        foreach (PaintProduct product in Products)
        {
            Console.WriteLine($"Product: {product.Name}");
            Console.WriteLine($"Final Price: ${product.GetFinalPrice():F2}");
            Console.WriteLine("--------------------");
        }

        Console.WriteLine($"Total Price: ${GetTotalOrderPrice():F2}");
    }
}