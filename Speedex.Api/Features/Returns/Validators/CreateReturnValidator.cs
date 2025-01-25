using FluentValidation;
using Speedex.Api.Features.Returns.Requests;

namespace Speedex.Api.Features.Returns.Validators;

public class CreateReturnValidator : AbstractValidator<CreateReturnBodyRequest>
{
    public CreateReturnValidator(IValidator<CreateReturnBodyRequest.CreateReturnBodyRequestReturnProduct> productValidator)
    {
        RuleFor(x => x.ReturnId)
            .NotEmpty();

        RuleFor(x => x.OrderId)
            .NotEmpty();

        RuleFor(x => x.ParcelId)
            .NotEmpty();

        RuleFor(x => x.Products)
            .NotEmpty()
            .ForEach(x => x.SetValidator(productValidator));
    }
}

public class CreateReturnBodyRequestReturnProductValidator : AbstractValidator<CreateReturnBodyRequest.CreateReturnBodyRequestReturnProduct>
{
    public CreateReturnBodyRequestReturnProductValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();

        RuleFor(x => x.Quantity)
            .GreaterThan(0);
    }
}