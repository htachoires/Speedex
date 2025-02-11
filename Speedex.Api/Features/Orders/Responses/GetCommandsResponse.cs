namespace Speedex.Api.Features.Orders.Responses;

public class GetOrdersResponse
{
    public IEnumerable<GetOrderItemResponse> Items { get; init; }

    public record GetOrderItemResponse
    {
        public string OrderId { get; init; }
        public IEnumerable<OrderProductResponse> Products { get; init; }
        public string DeliveryType { get; init; }
        public Recipient Recipient { get; init; }
        public string CreationDate { get; init; }
        public string UpdateDate { get; init; }
        public decimal TotalPrice { get; init; }
    }

    public record Recipient
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }
        public string PhoneNumber { get; init; }
        public string Address { get; init; }
        public string? AdditionalAddress { get; init; }
        public string City { get; init; }
        public string Country { get; init; }
    }

    public record OrderProductResponse
    {
        public string ProductId { get; init; }
        public int Quantity { get; init; }
    }
}