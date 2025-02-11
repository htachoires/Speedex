namespace Speedex.Api.Tests.Integration.Features.Orders.Request;

public class CreateOrderTestBodyRequest
{
    public string? DeliveryType { get; init; }
    public IEnumerable<ProductTestBodyRequest>? Products { get; init; }
    public RecipientTestBodyRequest? Recipient { get; init; }
    
    
    public record ProductTestBodyRequest
    {
        public string? ProductId { get; init; }
        public int? Quantity { get; init; }
        
        public decimal? TotalPrice { get; init; } 
    }

    public record RecipientTestBodyRequest
    {
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? Email { get; init; }
        public string? Phone { get; init; }
        public string? Address { get; init; }
        public string? AdditionalAddress { get; init; }
        public string? City { get; init; }
        public string? Country { get; init; }
    }
}