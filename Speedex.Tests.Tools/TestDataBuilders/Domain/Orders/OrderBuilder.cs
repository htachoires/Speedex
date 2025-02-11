using Speedex.Domain.Orders;

namespace Speedex.Tests.Tools.TestDataBuilders.Domain.Orders;

public class OrderBuilder
{
    private OrderId _orderId = new("fooOrderId");

    private List<OrderProduct> _products = new List<OrderProduct>
    {
        OrderProductBuilder.AnOrderProduct.Build()
    };

    private DeliveryType _deliveryType = DeliveryType.Standard;
    private RecipientBuilder _recipient = RecipientBuilder.ARecipient;
    private DateTime _creationDate = DateTime.Now;
    private DateTime _updateDate = DateTime.Now;

    public static OrderBuilder AnOrder => new();

    private OrderBuilder()
    {
    }

    public Order Build()
    {
        return new Order
        {
            OrderId = _orderId,
            Products = _products,
            DeliveryType = _deliveryType,
            Recipient = _recipient.Build(),
            CreationDate = _creationDate,
            UpdateDate = _updateDate,
        };
    }

    public OrderBuilder Id(OrderId id)
    {
        _orderId = id;
        return this;
    }
    
    public OrderBuilder WithProduct(OrderProduct productBuilder)
    {
        _products.Add(productBuilder);
        return this;
    }
}