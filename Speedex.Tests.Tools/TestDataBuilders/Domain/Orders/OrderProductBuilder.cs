using Speedex.Domain.Orders;
using Speedex.Domain.Products;

namespace Speedex.Tests.Tools.TestDataBuilders.Domain.Orders;

public class OrderProductBuilder
{
    private ProductId _productId = new("fooProductId");
    private int _quantity = 1;

    public static OrderProductBuilder AnOrderProduct => new();

    public OrderProduct Build()
    {
        return new OrderProduct
        {
            ProductId = _productId,
            Quantity = _quantity
        };
    }
    public OrderProductBuilder WithProductId(string productId)
    {
        _productId = new ProductId(productId); // Update ProductId
        return this; // Return this instance for method chaining
    }
}