using Speedex.Domain.Products;
using Speedex.Domain.Products.Repositories;
using Speedex.Domain.Products.Repositories.Dtos;

namespace Speedex.Infrastructure;

public class InMemoryProductRepository : IProductRepository
{
    private readonly Dictionary<ProductId, Product> _orders = new();

    public UpsertProductResult UpsertProduct(Product product)
    {
        if (!_orders.TryAdd(product.ProductId, product))
        {
            _orders[product.ProductId] = product;
        }

        return new UpsertProductResult
        {
            Status = UpsertProductResult.UpsertStatus.Success,
        };
    }

    public IEnumerable<Product> GetProducts(GetProductsDto query)
    {
        if (query.ProductId is not null)
        {
            return _orders.TryGetValue(query.ProductId, out var product) ? new List<Product> { product } : new List<Product>();
        }

        return _orders.Values
            .Skip((query.PageIndex - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();
    }
}