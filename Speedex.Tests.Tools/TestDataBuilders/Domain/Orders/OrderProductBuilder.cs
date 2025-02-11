using Speedex.Domain.Orders;
using Speedex.Domain.Products;

namespace Speedex.Tests.Tools.TestDataBuilders.Domain.Orders;

public class OrderProductBuilder
{
    private ProductId _productId = new("fooProductId");
    private int _quantity = 1;

    public static OrderProductBuilder AnOrderProduct => new();

    private OrderProductBuilder() {}

    public OrderProductBuilder WithProductId(ProductId productId)
    {
        _productId = productId;
        return this;
    }

    public OrderProductBuilder WithQuantity(int quantity)
    {
        _quantity = quantity;
        return this;
    }

    public OrderProduct Build()
    {
        return new OrderProduct
        {
            ProductId = _productId,
            Quantity = _quantity
        };
    }
}