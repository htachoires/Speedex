using FluentValidation;
using Speedex.Api.Features.Orders.Requests;
using Speedex.Domain.Orders;

namespace Speedex.Api.Features.Orders.Validators;

public class CreateOrderValidator : AbstractValidator<CreateOrderBodyRequest>
{
    public CreateOrderValidator(IValidator<CreateOrderBodyRequest.ProductBodyRequest> productValidator,
        IValidator<CreateOrderBodyRequest.RecipientBodyRequest> recipientValidator)
    {
        RuleFor(x => x.DeliveryType)
            .NotEmpty()
            .IsEnumName(typeof(DeliveryType), false);

        RuleFor(x => x.Products)
            .NotEmpty()
            .ForEach(x => x.SetValidator(productValidator));

        RuleFor(x => x.Recipient)
            .NotEmpty()
            .SetValidator(recipientValidator!);
    }
}

public class ProductValidator : AbstractValidator<CreateOrderBodyRequest.ProductBodyRequest>
{
    public ProductValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();

        RuleFor(x => x.Quantity)
            .GreaterThan(0);
    }
}

public class RecipientValidator : AbstractValidator<CreateOrderBodyRequest.RecipientBodyRequest>
{
    public RecipientValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty();

        RuleFor(x => x.Phone)
            .NotEmpty();

        RuleFor(x => x.Address)
            .NotEmpty();

        RuleFor(x => x.AdditionalAddress)
            .NotEmpty();

        RuleFor(x => x.City)
            .NotEmpty();
    }
}