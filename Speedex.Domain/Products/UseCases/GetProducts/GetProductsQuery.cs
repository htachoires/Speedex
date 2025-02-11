using Speedex.Domain.Commons;

namespace Speedex.Domain.Products.UseCases.GetProducts;

public record GetProductsQuery : IQuery
{
    public ProductId? ProductId { get; init; }
    public int? PageIndex { get; init; }
    public int? PageSize { get; init; }
    public string? Category { get; init; }
}