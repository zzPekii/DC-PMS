namespace PaintStore.Model;

public class Order
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public int UserId { get; set; }

    public List<PaintProduct> PaintProducts { get; set; } = new();

    public Order(int userId, List<PaintProduct> products)
    {
        UserId = userId;
        PaintProducts = products;
    }

    public decimal GetTotalOrderPrice()
    {
        decimal total = 0;

        foreach (PaintProduct product in PaintProducts)
        {
            total += product.GetFinalPrice();
        }

        return total;
    }

    public void RemoveProduct(int productId)
    {
        PaintProduct? product = PaintProducts.Find(p => p.Id == productId);

        if (product != null)
        {
            PaintProducts.Remove(product);
        }
    }

    public PaintProduct? GetMostExpensivePaintProduct()
    {
        if (PaintProducts.Count == 0)
            return null;

        PaintProduct mostExpensive = PaintProducts[0];

        foreach (PaintProduct product in PaintProducts)
        {
            if (product.Price > mostExpensive.Price)
            {
                mostExpensive = product;
            }
        }

        return mostExpensive;
    }

    public List<PaintProduct> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
    {
        List<PaintProduct> result = new();

        foreach (PaintProduct product in PaintProducts)
        {
            if (product.Price > minPrice && product.Price < maxPrice)
            {
                result.Add(product);
            }
        }

        return result;
    }

    public Dictionary<PaintType, decimal> GetTotalPriceByPaintType()
    {
        Dictionary<PaintType, decimal> result = new();

        foreach (PaintProduct product in PaintProducts)
        {
            if (result.ContainsKey(product.Type))
            {
                result[product.Type] += product.Price;
            }
            else
            {
                result.Add(product.Type, product.Price);
            }
        }

        return result;
    }

    public void DisplayOrder()
    {
        Console.WriteLine("===== Order =====");
        Console.WriteLine($"Order ID: {Id}");
        Console.WriteLine($"Created Date: {CreatedDate}");
        Console.WriteLine($"User ID: {UserId}");

        foreach (PaintProduct product in PaintProducts)
        {
            Console.WriteLine($"Product: {product.Name}");
            Console.WriteLine($"Final Price: ${product.GetFinalPrice():F2}");
            Console.WriteLine("--------------------");
        }

        Console.WriteLine($"Total Price: ${GetTotalOrderPrice():F2}");
    }
}