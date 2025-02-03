using Speedex.Domain.Orders;
using Speedex.Domain.Parcels;
using Speedex.Domain.Products;
using static Speedex.Tests.Tools.TestDataBuilders.Domain.Parcels.ParcelProductBuilder;

namespace Speedex.Tests.Tools.TestDataBuilders.Domain.Parcels;

public class ParcelBuilder
{
    private ParcelId _parcelId = new ParcelId(Guid.NewGuid().ToString());
    private ParcelStatus _parcelStatus = ParcelStatus.Created;
    private OrderId _orderId = new OrderId(Guid.NewGuid().ToString());
    private IEnumerable<ParcelProduct> _products = [AParcelProduct.Build()];
    private DateTime _creationDate = DateTime.Now;
    private DateTime _updateDate = DateTime.Now;

    public static ParcelBuilder AParcel => new();

    public Parcel Build()
    {
        return new Parcel
        {
            ParcelId = _parcelId,
            ParcelStatus = _parcelStatus,
            OrderId = _orderId,
            Products = _products,
            CreationDate = _creationDate,
            UpdateDate = _updateDate,
        };
    }

    public ParcelBuilder Id(ParcelId id)
    {
        _parcelId = id;
        return this;
    }
}

public class ParcelProductBuilder
{
    private ProductId _productId = new ProductId(Guid.NewGuid().ToString());
    private int _quantity = 1;

    public static ParcelProductBuilder AParcelProduct => new();

    public ParcelProduct Build()
    {
        return new ParcelProduct
        {
            ProductId = _productId,
            Quantity = _quantity,
        };
    }

    public ParcelProductBuilder Id(ProductId id)
    {
        _productId = id;
        return this;
    }

    public ParcelProductBuilder WithQuantity(int quantity)
    {
        _quantity = quantity;
        return this;
    }
}