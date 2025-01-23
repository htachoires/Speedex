using Speedex.Domain.Parcels;
using Speedex.Domain.Parcels.Repositories;
using Speedex.Domain.Parcels.Repositories.Dtos;

namespace Speedex.Infrastructure;

public class InMemoryParcelRepository : IParcelRepository
{
    private readonly Dictionary<ParcelId, Parcel> _orders = new();

    public UpsertParcelResult UpsertParcel(Parcel order)
    {
        if (!_orders.TryAdd(order.ParcelId, order))
        {
            _orders[order.ParcelId] = order;
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
            return _orders.TryGetValue(query.ParcelId, out var order) ? new List<Parcel> { order } : new List<Parcel>();
        }

        return _orders.Values
            .Skip((query.PageIndex - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();
    }
}