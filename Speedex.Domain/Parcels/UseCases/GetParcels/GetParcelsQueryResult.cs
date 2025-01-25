using Speedex.Domain.Commons;

namespace Speedex.Domain.Parcels.UseCases.GetParcels;

public record GetParcelsQueryResult : IQueryResult
{
    public IEnumerable<Parcel> Items { get; init; }
}