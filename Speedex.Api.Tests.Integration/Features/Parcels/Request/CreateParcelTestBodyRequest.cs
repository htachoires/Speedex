namespace Speedex.Api.Tests.Integration.Features.Parcels.Request;

public class CreateParcelTestBodyRequest
{
    public string? OrderId { get; init; }
    public IEnumerable<ParcelProductCreateParcelTestBodyRequest>? Products { get; init; }

    public record ParcelProductCreateParcelTestBodyRequest
    {
        public string? ProductId { get; init; }
        public int? Quantity { get; init; }
    }
}