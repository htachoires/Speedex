using FluentValidation;
using Speedex.Api.Features.Products.Requests;
using Speedex.Domain.Products;

namespace Speedex.Api.Features.Products.Validators;

public class CreateProductValidator : AbstractValidator<CreateProductBodyRequest>
{
    public CreateProductValidator(IValidator<CreateProductBodyRequest.PriceGetProductBodyRequest> priceValidator,
        IValidator<CreateProductBodyRequest.DimensionsGetProductBodyRequest> dimensionsValidator,
        IValidator<CreateProductBodyRequest.WeightGetProductBodyRequest> weightValidator)
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Description)
            .NotEmpty();

        RuleFor(x => x.Category)
            .NotEmpty();

        RuleFor(x => x.SecondLevelCategory)
            .NotEmpty();

        RuleFor(x => x.ThirdLevelCategory)
            .NotEmpty();

        RuleFor(x => x.Price)
            .SetValidator(priceValidator);

        RuleFor(x => x.Dimensions)
            .SetValidator(dimensionsValidator);

        RuleFor(x => x.Weight)
            .SetValidator(weightValidator);
    }
}

public class
    PriceGetProductBodyRequestValidator : AbstractValidator<CreateProductBodyRequest.PriceGetProductBodyRequest>
{
    public PriceGetProductBodyRequestValidator()
    {
        RuleFor(x => x.Amount)
            .NotEmpty();

        RuleFor(x => x.Currency)
            .IsEnumName(typeof(Currency), false);
    }
}

public class
    WeightGetProductBodyRequestValidator : AbstractValidator<CreateProductBodyRequest.WeightGetProductBodyRequest>
{
    public WeightGetProductBodyRequestValidator()
    {
        RuleFor(x => x.Value)
            .NotEmpty();

        RuleFor(x => x.Unit)
            .IsEnumName(typeof(WeightUnit), false)
            .NotEmpty();
    }
}

public class
    DimensionsGetProductBodyRequestValidator : AbstractValidator<
    CreateProductBodyRequest.DimensionsGetProductBodyRequest>
{
    public DimensionsGetProductBodyRequestValidator()
    {
        RuleFor(x => x.X)
            .NotEmpty();

        RuleFor(x => x.Y)
            .NotEmpty();

        RuleFor(x => x.Z)
            .NotEmpty();

        RuleFor(x => x.Unit)
            .IsEnumName(typeof(DimensionUnit), false)
            .NotEmpty();
    }
}