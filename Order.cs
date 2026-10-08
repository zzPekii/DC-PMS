using System.Security.Cryptography;

namespace DCPMS;

public class Order
{
    public DateTime CreatedAt { get; set; }
    public List<PaintProduct> Products { get; set; }

    public Order(List<PaintProduct> products)
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

    // remove product instance by its id
    public void RemoveProduct(int productId)
    {
        PaintProduct product = Products.Find(p => p.ProductId == productId);
        if (product != null)
        {
            Products.Remove(product);
        }
    }

    public PaintProduct GetMostExpensivePaintProduct()
    {
        PaintProduct mostExpensive = Products[0];

        foreach (PaintProduct product in Products)
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
        List<PaintProduct> result = new List<PaintProduct>();
        foreach (PaintProduct product in Products)
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
        Dictionary<PaintType, decimal> result = new Dictionary<PaintType, decimal>();
        foreach (PaintProduct product in Products)
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