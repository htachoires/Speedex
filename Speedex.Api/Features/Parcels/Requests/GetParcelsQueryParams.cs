namespace Speedex.Api.Features.Parcels.Requests;

public record GetParcelsQueryParams
{
    public string? ParcelId { get; init; }
    public int? PageIndex { get; init; }
    public int? PageSize { get; init; }
}