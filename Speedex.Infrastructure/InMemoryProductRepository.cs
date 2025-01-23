using Speedex.Domain.Products;
using Speedex.Domain.Products.Repositories;
using Speedex.Domain.Products.Repositories.Dtos;

namespace Speedex.Infrastructure;

public class InMemoryProductRepository : IProductRepository
{
    private readonly Dictionary<ProductId, Product> _orders = new();

    public UpsertProductResult UpsertProduct(Product order)
    {
        if (!_orders.TryAdd(order.ProductId, order))
        {
            _orders[order.ProductId] = order;
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
            return _orders.TryGetValue(query.ProductId, out var order) ? new List<Product> { order } : new List<Product>();
        }

        return _orders.Values
            .Skip((query.PageIndex - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();
    }
}