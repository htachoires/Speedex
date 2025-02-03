using FluentValidation;
using Speedex.Domain.Parcels.Repositories;

namespace Speedex.Domain.Parcels.Validators;

public class IsExistingParcelValidator : AbstractValidator<ParcelId>
{
    public IsExistingParcelValidator(IParcelRepository parcelRepository)
    {
        RuleFor(x => x)
            .MustAsync(async (parcelId, cancellationToken) => await parcelRepository.IsExistingParcel(parcelId, cancellationToken))
            .WithErrorCode(nameof(IsExistingParcelValidator))
            .WithMessage("Parcel with id {PropertyValue} does not exist");
    }
}