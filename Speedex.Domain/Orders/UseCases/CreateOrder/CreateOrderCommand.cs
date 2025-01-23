using Speedex.Domain.Commons;

namespace Speedex.Domain.Orders.UseCases.CreateOrder;

public record CreateOrderCommand : ICommand
{
    public IEnumerable<Product> Products { get; init; }
    public DeliveryType DeliveryType { get; init; }
    public CreateOrderRecipient Recipient { get; init; }

    public record Product
    {
        public string ProductId { get; init; }
        public int Quantity { get; init; }
    }

    public record CreateOrderRecipient
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