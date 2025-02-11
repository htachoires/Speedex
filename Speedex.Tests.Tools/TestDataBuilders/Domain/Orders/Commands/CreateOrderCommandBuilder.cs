using Speedex.Domain.Orders;
using Speedex.Domain.Orders.UseCases.CreateOrder;

namespace Speedex.Tests.Tools.TestDataBuilders.Domain.Orders.Commands;

public class CreateOrderCommandBuilder
{
    private List<CreateOrderCommandProductBuilder> _products =
    [
        CreateOrderCommandProductBuilder.ACreateOrderCommandProduct
    ];

    private CreateOrderRecipientBuilder _recipient = CreateOrderRecipientBuilder.ACreateOrderRecipient;
    private DeliveryType _deliveryType = DeliveryType.Standard;

    public static CreateOrderCommandBuilder ACreateOrderCommand => new();

    private CreateOrderCommandBuilder()
    {
    }

    public CreateOrderCommand Build()
    {
        return new CreateOrderCommand
        {
            Products = _products.Select(x => x.Build()),
            Recipient = _recipient.Build(),
            DeliveryType = _deliveryType
        };
    }

    public CreateOrderCommandBuilder WithProduct(CreateOrderCommandProductBuilder product)
    {
        _products.Clear();
        _products.Add(product);
        return this;
    }

    public CreateOrderCommandBuilder AddProduct(CreateOrderCommandProductBuilder product)
    {
        _products.Add(product);
        return this;
    }
    
    public CreateOrderCommandBuilder WithProducts(params CreateOrderCommandProductBuilder[] products)
    {
        _products = products.ToList();
        return this;
    }

    public CreateOrderCommandBuilder WithRecipient(CreateOrderRecipientBuilder recipient)
    {
        _recipient = recipient;
        return this;
    }

    public CreateOrderCommandBuilder WithDeliveryType(DeliveryType deliveryType)
    {
        _deliveryType = deliveryType;
        return this;
    }
}