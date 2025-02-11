using Speedex.Domain.Orders;
using Speedex.Domain.Products;

namespace Speedex.Tests.Tools.TestDataBuilders.Domain.Orders;

public class OrderBuilder
{
    private OrderId _orderId = new("fooOrderId");

    private IEnumerable<OrderProductBuilder> _products = new List<OrderProductBuilder>
    {
        OrderProductBuilder.AnOrderProduct
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
            Products = _products.Select(x => x.Build()).ToList(),
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
    
    public OrderBuilder WithProducts(IEnumerable<ProductId> productIds)
    {
        _products = productIds
            .Select(id => OrderProductBuilder.AnOrderProduct.WithProductId(id))
            .ToList();
        return this;
    }
}