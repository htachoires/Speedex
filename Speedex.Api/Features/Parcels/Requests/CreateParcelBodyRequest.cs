namespace Speedex.Api.Features.Parcels.Requests;

public record CreateParcelBodyRequest
{
    public string? OrderId { get; init; }
    public IEnumerable<ParcelProductCreateParcelBodyRequest>? Products { get; init; }

    public record ParcelProductCreateParcelBodyRequest
    {
        public string? ProductId { get; init; }
        public int? Quantity { get; init; }
    }
}