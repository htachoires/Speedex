namespace Speedex.Api.Features.Parcels.Responses;

public class GetParcelsResponse
{
    public IEnumerable<GetParcelItemResponse> Items { get; init; }

    public record GetParcelItemResponse
    {
        public string ParcelId { get; init; }
        public string ParcelStatus { get; init; }
        public string OrderId { get; init; }
        public IEnumerable<ParcelProductItemResponse> Products { get; init; }
        public string CreationDate { get; init; }
        public string UpdateDate { get; init; }
    }

    public record ParcelProductItemResponse
    {
        public string ProductId { get; init; }
        public int Quantity { get; init; }
    }
}