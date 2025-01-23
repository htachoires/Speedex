using FluentValidation;
using Speedex.Api.Features.Orders.Requests;
using Speedex.Domain.Orders;

namespace Speedex.Api.Features.Orders.Validators;

public class CreateOrderValidator : AbstractValidator<CreateOrderBodyRequest>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.DeliveryType)
            .NotEmpty()
            .IsEnumName(typeof(DeliveryType), false);

        RuleFor(x => x.Products)
            .NotEmpty();

        RuleForEach(x => x.Products)
            .SetValidator(new ProductValidator());
    }
}

public class ProductValidator : AbstractValidator<CreateOrderBodyRequest.Product>
{
    public ProductValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();

        RuleFor(x => x.Quantity)
            .GreaterThan(0);
    }
}