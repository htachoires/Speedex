using FluentValidation;
using Speedex.Domain.Products.Repositories;

namespace Speedex.Domain.Products.Validators;

public class IsExistingProductValidator : AbstractValidator<ProductId>
{
    public IsExistingProductValidator(IProductRepository productRepository)
    {
        RuleFor(x => x)
            .MustAsync(async (productId, cancellationToken) => await productRepository.IsExistingProduct(productId, cancellationToken))
            .WithErrorCode(nameof(IsExistingProductValidator))
            .WithMessage("Product with id {PropertyValue} does not exist");
    }
}