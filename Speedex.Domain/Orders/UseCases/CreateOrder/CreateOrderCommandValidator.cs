using FluentValidation;
using Speedex.Domain.Products;

namespace Speedex.Domain.Orders.UseCases.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator(IValidator<ProductId> productValidator)
    {
        RuleFor(x => x.Products.Select(p => p.ProductId))
            .ForEach(x => x.SetValidator(productValidator));
    }
}