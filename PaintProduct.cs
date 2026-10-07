namespace DCPMS;

public class PaintProduct : IBuyable
{
    public string Name { get; set; }
    public PaintType Type { get; set; }

    public PaintSpecification Specification { get; set; }

    public Brand Brand { get; set; }
    public decimal Price { get; set; }
    // TaxRate readonly, it cannot change when instance is created
    public readonly decimal TaxRate = 0.10m;
    public const decimal DefaultDiscount = 0.05m;

    public PaintProduct (string name, PaintType type, PaintSpecification specification, Brand brand, decimal price)
    {
        Name = name;
        Type = type;
        Specification = specification;
        Brand = brand;
        Price = price;
    }

    public decimal GetFinalPrice()
    {
        decimal discountedPrice = Price * (1 - DefaultDiscount);
        decimal finalPrice = discountedPrice * (1 + TaxRate);
        return finalPrice;
    }

    public decimal GetMaxDiscount(decimal discount)
    {
        if (discount > DefaultDiscount)
        {
            return DefaultDiscount;
        }
        return discount;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Name: {Name}");
        Console.WriteLine($"Type: {Type}");
        Console.WriteLine($"Brand: {Brand.Name}");
        Console.WriteLine($"Price: ${Price:F2}");
        Specification.DisplaySpecification();
        Console.WriteLine($"Final Price: ${GetFinalPrice():F2}");
    }
}