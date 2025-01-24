using FluentValidation;
using Speedex.Api.Features.Products.Requests;

namespace Speedex.Api.Features.Products.Validators;

public class GetProductsValidator : AbstractValidator<GetProductsQueryParams>
{
    public GetProductsValidator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThan(0)
            .Unless(x => x.PageIndex is null);

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .Unless(x => x.PageSize is null);
    }
}