using Speedex.Domain.Orders;
using Speedex.Domain.Parcels;

namespace Speedex.Data.Generators;

public class ParcelsGenerator : IDataGenerator<ParcelId, Parcel>
{
    private readonly IDataGenerator<OrderId, Order> _orderGenerator;
    public Dictionary<ParcelId, Parcel> Data { get; private set; }
    private readonly Random _random;

    public ParcelsGenerator(IDataGenerator<OrderId, Order> orderGenerator)
    {
        _orderGenerator = orderGenerator;
        _random = new Random();
    }

    public void GenerateData(int nbElements)
    {
        Data = Enumerable
            .Range(0, nbElements)
            .Select(_ => GenerateParcel())
            .ToDictionary(x => x.ParcelId);
    }

    private Parcel GenerateParcel()
    {
        var order = _orderGenerator.Data.ElementAt(_random.Next(_orderGenerator.Data.Count)).Value;

        return new Parcel
        {
            ParcelId = new ParcelId(Guid.NewGuid().ToString()),
            ParcelStatus = ParcelStatus.Preparing,
            OrderId = order.OrderId,
            Products = order.Products.Select(x => new ParcelProduct()
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
            }),
            CreationDate = DateTime.Now,
            UpdateDate = DateTime.Now
        };
    }
}