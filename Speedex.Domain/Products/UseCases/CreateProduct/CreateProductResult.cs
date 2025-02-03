using Speedex.Domain.Commons;

namespace Speedex.Domain.Products.UseCases.CreateProduct;

public record CreateProductResult : ICommandResult
{
    public ProductId ProductId { get; init; }
    public bool Success { get; init; }
    public List<ValidationError> Errors { get; set; }

    public record ValidationError
    {
        public string Message { get; init; }
        public string PropertyName { get; init; }
        public string Code { get; set; }
    }
}