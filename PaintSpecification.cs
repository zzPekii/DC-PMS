namespace DCPMS;

public class PaintSpecification
{

    public string Color { get; set; }
    public int SizeInLiters { get; set; }
    public PaintSpecification(string color, int sizeInLiters)
    {
        Color = color;
        SizeInLiters = sizeInLiters;
    }

    public void DisplaySpecification()
    {
        Console.WriteLine($"Color: {Color}");
        Console.WriteLine($"Size {SizeInLiters}");
    }

}