using FluentValidation;
using Speedex.Api.Features.Parcels.Requests;

namespace Speedex.Api.Features.Parcels.Validators;

public class CreateParcelValidator : AbstractValidator<CreateParcelBodyRequest>
{
    public CreateParcelValidator(IValidator<CreateParcelBodyRequest.ParcelProductCreateParcelBodyRequest> productValidator)
    {
        RuleFor(x => x.OrderId)
            .NotEmpty();

        RuleFor(x => x.Products)
            .NotEmpty()
            .ForEach(x => x.SetValidator(productValidator));
    }
}

public class ParcelProductCreateParcelBodyRequestValidator : AbstractValidator<CreateParcelBodyRequest.ParcelProductCreateParcelBodyRequest>
{
    public ParcelProductCreateParcelBodyRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();

        RuleFor(x => x.Quantity)
            .GreaterThan(0);
    }
}