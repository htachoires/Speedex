using Speedex.Domain.Parcels.Repositories.Dtos;

namespace Speedex.Domain.Parcels.Repositories;

public interface IParcelRepository
{
    UpsertParcelResult UpsertParcel(Parcel parcel);

    IEnumerable<Parcel> GetParcels(GetParcelsDto query);

    Task<bool> IsExistingParcel(ParcelId parcelId, CancellationToken cancellationToken);
}