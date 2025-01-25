using Speedex.Domain.Parcels;
using Speedex.Domain.Parcels.Repositories;
using Speedex.Domain.Parcels.Repositories.Dtos;

namespace Speedex.Infrastructure;

public class InMemoryParcelRepository : IParcelRepository
{
    private readonly Dictionary<ParcelId, Parcel> _orders = new();

    public UpsertParcelResult UpsertParcel(Parcel parcel)
    {
        if (!_orders.TryAdd(parcel.ParcelId, parcel))
        {
            _orders[parcel.ParcelId] = parcel;
        }

        return new UpsertParcelResult
        {
            Status = UpsertParcelResult.UpsertStatus.Success,
        };
    }

    public IEnumerable<Parcel> GetParcels(GetParcelsDto query)
    {
        if (query.ParcelId is not null)
        {
            return _orders.TryGetValue(query.ParcelId, out var parcel) ? new List<Parcel> { parcel } : new List<Parcel>();
        }

        return _orders.Values
            .Skip((query.PageIndex - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();
    }
}