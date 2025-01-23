namespace Speedex.Api.Features.Orders.Requests;

public record CreateOrderBodyRequest
{
    public string? DeliveryType { get; init; }
    public IEnumerable<Product>? Products { get; init; }
    public RecipientBodyRequest? Recipient { get; init; }

    public record Product
    {
        public string? ProductId { get; init; }
        public int? Quantity { get; init; }
    }

    public record RecipientBodyRequest
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }
        public string Phone { get; init; }
        public string Address { get; init; }
        public string AdditionalAddress { get; init; }
        public string City { get; init; }
        public string Country { get; init; }
    }
}