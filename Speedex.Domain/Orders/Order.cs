using Speedex.Domain.Products;

namespace Speedex.Domain.Orders;

public record Order
{
    public OrderId OrderId { get; init; }
    public IEnumerable<OrderProduct> Products { get; init; }
    public DeliveryType DeliveryType { get; init; }
    public Recipient Recipient { get; init; }
    public DateTime CreationDate { get; init; }
    public DateTime UpdateDate { get; init; }
}

public enum DeliveryType
{
    Standard,
    Express,
    Premium,
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

public record OrderProduct
{
    public ProductId ProductId { get; init; }
    public int Quantity { get; init; }
}

public record OrderId(string Value);