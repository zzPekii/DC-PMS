
using PaintStore.API.Data;
using PaintStore.API.Interfaces.Repositories;
using PaintStore.Model;

namespace PaintStore.API.Repositories;

public class PaintProductRepository : IPaintProductRepository
{
    private readonly InMemoryStore _store;

    public PaintProductRepository(InMemoryStore store)
    {
        _store = store;
    }

    public List<PaintProduct> GetPaintProducts()
    {
        return _store.PaintProducts;
    }

    public PaintProduct? GetPaintProductById(int id)
    {
        return _store.PaintProducts.FirstOrDefault(p => p.Id == id);
    }

    public List<PaintProduct> SearchPaintProducts(
        string? name,
        string? brand,
        decimal? minPrice,
        decimal? maxPrice)
    {
        IEnumerable<PaintProduct> products = _store.PaintProducts;

        if (!string.IsNullOrWhiteSpace(name))
        {
            products = products.Where(p =>
                p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(brand))
        {
            products = products.Where(p =>
                p.Brand.Name.Contains(brand, StringComparison.OrdinalIgnoreCase));
        }

        if (minPrice.HasValue)
        {
            products = products.Where(p => p.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            products = products.Where(p => p.Price <= maxPrice.Value);
        }

        return products.ToList();
    }

    public PaintProduct AddPaintProduct(PaintProduct product)
    {
        int nextId = _store.PaintProducts.Count == 0
            ? 1
            : _store.PaintProducts.Max(p => p.Id) + 1;

        product.SetId(nextId);
        _store.PaintProducts.Add(product);

        return product;
    }

    public bool UpdatePaintProduct(PaintProduct product)
    {
        PaintProduct? existingProduct = GetPaintProductById(product.Id);

        if (existingProduct == null)
        {
            return false;
        }

        existingProduct.Name = product.Name;
        existingProduct.Type = product.Type;
        existingProduct.Specification = product.Specification;
        existingProduct.Brand = product.Brand;
        existingProduct.Price = product.Price;
        existingProduct.Stock = product.Stock;

        return true;
    }

    public bool DeletePaintProduct(int id)
    {
        PaintProduct? product = GetPaintProductById(id);

        if (product == null)
        {
            return false;
        }

        return _store.PaintProducts.Remove(product);
    }
}
