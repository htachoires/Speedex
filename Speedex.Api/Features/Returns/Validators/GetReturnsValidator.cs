using FluentValidation;
using Speedex.Api.Features.Returns.Requests;

namespace Speedex.Api.Features.Returns.Validators;

public class GetReturnsValidator : AbstractValidator<GetReturnsQueryParams>
{
    public GetReturnsValidator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThan(0)
            .Unless(x => x.PageIndex is null);

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .Unless(x => x.PageSize is null);
    }
}