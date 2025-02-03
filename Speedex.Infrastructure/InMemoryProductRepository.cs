using Speedex.Domain.Products;
using Speedex.Domain.Products.Repositories;
using Speedex.Domain.Products.Repositories.Dtos;

namespace Speedex.Infrastructure;

public class InMemoryProductRepository : IProductRepository
{
    private readonly Dictionary<ProductId, Product> _products = new();

    public UpsertProductResult UpsertProduct(Product product)
    {
        if (!_products.TryAdd(product.ProductId, product))
        {
            _products[product.ProductId] = product;
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
            return _products.TryGetValue(query.ProductId, out var product) ? new List<Product> { product } : new List<Product>();
        }

        return _products.Values
            .Skip((query.PageIndex - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();
    }

    public Task<bool> IsExistingProduct(ProductId productId, CancellationToken cancellationToken)
    {
        return Task.FromResult(_products.ContainsKey(productId));
    }
}