namespace Speedex.Domain.Parcels.Repositories.Dtos;

public record GetParcelsDto
{
    public ParcelId? ParcelId { get; init; }
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
}