using Speedex.Domain.Commons;

namespace Speedex.Domain.Parcels.UseCases.GetParcels;

public record GetParcelsQuery : IQuery
{
    public ParcelId? ParcelId { get; init; }
    public int? PageIndex { get; init; }
    public int? PageSize { get; init; }
}