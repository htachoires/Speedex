using Speedex.Domain.Orders.UseCases.CreateOrder;
using Speedex.Domain.Products;

namespace Speedex.Tests.Tools.TestDataBuilders.Domain.Orders.Commands;

public class CreateOrderCommandProductBuilder
{
    private ProductId _productId = new("defaultProductId");
    private int _quantity = 1;
    private decimal _length = 1.0m;
    private decimal _width = 1.0m;
    private decimal _height = 1.0m;

    public static CreateOrderCommandProductBuilder ACreateOrderCommandProduct => new();

    private CreateOrderCommandProductBuilder()
    {
    }

    public CreateOrderCommand.Product Build()
    {
        return new CreateOrderCommand.Product
        {
            ProductId = _productId,
            Quantity = _quantity,
            Length = _length,
            Width = _width,
            Height = _height
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
    
    public CreateOrderCommandProductBuilder WithDimensions(decimal length, decimal width, decimal height)
    {
        _length = length;
        _width = width;
        _height = height;
        return this;
    }
}