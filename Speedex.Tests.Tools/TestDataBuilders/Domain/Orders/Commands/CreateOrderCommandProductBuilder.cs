using Speedex.Domain.Orders.UseCases.CreateOrder;
using Speedex.Domain.Products;

namespace Speedex.Tests.Tools.TestDataBuilders.Domain.Orders.Commands;

public class CreateOrderCommandProductBuilder
{
    private ProductId _productId = new("defaultProductId");
    private int _quantity = 1;

    public static CreateOrderCommandProductBuilder ACreateOrderCommandProduct => new();

    private CreateOrderCommandProductBuilder()
    {
    }

    public CreateOrderCommand.Product Build()
    {
        return new CreateOrderCommand.Product
        {
            ProductId = _productId,
            Quantity = _quantity
        };
    }

    public CreateOrderCommandProductBuilder WithProductId(ProductId productId)
    {
        _productId = productId;
        return this;
    }

    public CreateOrderCommandProductBuilder WithQuantity(int quantity)
    {
        _quantity = quantity;
        return this;
    }
}