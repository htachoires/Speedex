using Speedex.Domain.Commons;

namespace Speedex.Domain.Products.UseCases.CreateProduct;

public record CreateProductResult : ICommandResult
{
    public ProductId ProductId { get; init; }
    public bool Success { get; init; }
}