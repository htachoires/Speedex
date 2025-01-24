using Speedex.Domain.Commons;

namespace Speedex.Domain.Products.UseCases.GetProducts;

public record GetProductsQueryResult : IQueryResult
{
    public IEnumerable<Product> Items { get; init; }
}