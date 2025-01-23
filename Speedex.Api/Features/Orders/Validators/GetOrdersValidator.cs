using FluentValidation;
using Speedex.Api.Features.Orders.Requests;

namespace Speedex.Api.Features.Orders.Validators;

public class GetOrdersValidator : AbstractValidator<GetOrdersQueryParams>
{
    public GetOrdersValidator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThan(0)
            .Unless(x => x.PageIndex is null);

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .Unless(x => x.PageSize is null);
    }
}