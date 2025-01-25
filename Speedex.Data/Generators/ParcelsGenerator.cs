using Speedex.Domain.Orders;
using Speedex.Domain.Parcels;

namespace Speedex.Data.Generators;

public class ParcelsGenerator(IDataGenerator<OrderId, Order> orderGenerator) : IDataGenerator<ParcelId, Parcel>
{
    public Dictionary<ParcelId, Parcel> Data { get; private set; }
    private readonly Random _random = new();

    public void GenerateData(int nbElements)
    {
        Data = Enumerable
            .Range(0, nbElements)
            .Select(_ => GenerateParcel())
            .ToDictionary(x => x.ParcelId);
    }

    private Parcel GenerateParcel()
    {
        var order = orderGenerator.Data.ElementAt(_random.Next(orderGenerator.Data.Count)).Value;

        return new Parcel
        {
            ParcelId = new ParcelId(Guid.NewGuid().ToString()),
            ParcelStatus = (ParcelStatus)_random.Next(0, 4),
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