using Domain.Products;

namespace Domain.Commands;

public record Command
{
    public CommandId CommandId { get; init; }
    public IEnumerable<ProductCommand> Products { get; init; }
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
    public string Phone { get; init; }
    public string Address { get; init; }
    public string AdditionalAddress { get; init; }
    public string City { get; init; }
    public string Country { get; init; }
}

public record ProductCommand
{
    public ProductId ProductId { get; init; }
    public int Quantity { get; init; }
}

public record CommandId(string Value);