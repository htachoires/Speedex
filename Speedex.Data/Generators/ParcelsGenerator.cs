using System.Collections.Concurrent;
using Speedex.Domain.Orders;
using Speedex.Domain.Parcels;

namespace Speedex.Data.Generators;

public class ParcelsGenerator(IDataGenerator<OrderId, Order> orderGenerator) : IDataGenerator<ParcelId, Parcel>
{
    public Dictionary<ParcelId, Parcel> Data { get; private set; }
    private readonly Random _random = new();

    public void GenerateData(int nbElements)
    {
        var concurrentData = new ConcurrentDictionary<ParcelId, Parcel>();

        Enumerable
            .Range(0, nbElements)
            .AsParallel()
            .ForAll(
                x =>
                {
                    var product = GenerateParcel(x);
                    concurrentData.TryAdd(product.ParcelId, product);
                });

        Data = concurrentData.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    private Parcel GenerateParcel(int index)
    {
        var order = orderGenerator.Data.ElementAt(_random.Next(orderGenerator.Data.Count)).Value;

        return new Parcel
        {
            ParcelId = new ParcelId($"PA_{index}_{GenerateHexadecimal(10)}"),
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