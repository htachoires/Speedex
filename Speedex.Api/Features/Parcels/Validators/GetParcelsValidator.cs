using FluentValidation;
using Speedex.Api.Features.Parcels.Requests;

namespace Speedex.Api.Features.Parcels.Validators;

public class GetParcelsValidator : AbstractValidator<GetParcelsQueryParams>
{
    public GetParcelsValidator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThan(0)
            .Unless(x => x.PageIndex is null);

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .Unless(x => x.PageSize is null);
    }
}