using PaintStore.API.Interfaces.Repositories;
using PaintStore.API.Interfaces.Services;
using PaintStore.Model;

namespace PaintStore.API.Services;

public class PaintProductService : IPaintProductService
{
    private readonly IPaintProductRepository _paintProductRepository;

    public PaintProductService(
        IPaintProductRepository paintProductRepository)
    {
        _paintProductRepository = paintProductRepository;
    }

    public List<PaintProduct> GetPaintProducts()
    {
        return _paintProductRepository.GetPaintProducts();
    }

    public PaintProduct? GetPaintProductById(int id)
    {
        return _paintProductRepository.GetPaintProductById(id);
    }

    public List<PaintProduct> SearchPaintProducts(string? name, string? brand, decimal? minPrice, decimal? maxPrice)
    {
        return _paintProductRepository.SearchPaintProducts(
            name, brand, minPrice, maxPrice);
    }

    public string? ValidatePaintProduct(PaintProduct product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
        {
            return "Product name cannot be empty";
        }

        if (product.Price <= 0)
        {
            return "Price must be greater than 0";
        }

        if (product.Stock < 0)
        {
            return "Stock cannot be negative";
        }

        if (product.Brand == null ||
            string.IsNullOrWhiteSpace(product.Brand.Name))
        {
            return "Brand cannot be empty";
        }

        return null;
    }

    public PaintProduct AddPaintProduct(PaintProduct product)
    {
        return _paintProductRepository.AddPaintProduct(product);
    }

    public bool UpdatePaintProduct(PaintProduct product)
    {
        return _paintProductRepository.UpdatePaintProduct(product);
    }

    public bool DeletePaintProduct(int id)
    {
        return _paintProductRepository.DeletePaintProduct(id);
    }
}
