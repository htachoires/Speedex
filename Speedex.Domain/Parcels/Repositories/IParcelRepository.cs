using Speedex.Domain.Parcels.Repositories.Dtos;

namespace Speedex.Domain.Parcels.Repositories;

public interface IParcelRepository
{
    public UpsertParcelResult UpsertParcel(Parcel parcel);
    public IEnumerable<Parcel> GetParcels(GetParcelsDto query);
}