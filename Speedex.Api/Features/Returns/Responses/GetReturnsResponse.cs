namespace Speedex.Api.Features.Returns.Responses;

public class GetReturnsResponse
{
    public IEnumerable<GetReturnItemResponse> Items { get; init; }

    public class GetReturnItemResponse
    {
        public string ReturnId { get; init; }
        public string Status { get; init; }
        public string ParcelId { get; init; }
        public string OrderId { get; init; }
        public IEnumerable<ReturnProductGetReturnItemResponse> Products { get; init; }
        public string CreationDate { get; init; }
        public string UpdateDate { get; init; }

        public record ReturnProductGetReturnItemResponse
        {
            public string ProductId { get; init; }
            public int Quantity { get; init; }
        }
    }
}