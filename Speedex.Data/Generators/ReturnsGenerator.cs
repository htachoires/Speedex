using System.Collections.Concurrent;
using Speedex.Domain.Parcels;
using Speedex.Domain.Returns;

namespace Speedex.Data.Generators;

public class ReturnsGenerator(IDataGenerator<ParcelId, Parcel> parcelsGenerator) : IDataGenerator<ReturnId, Return>
{
    public Dictionary<ReturnId, Return> Data { get; private set; }
    private readonly Random _random = new();

    public void GenerateData(int nbElements)
    {
        var concurrentData = new ConcurrentDictionary<ReturnId, Return>();

        Enumerable
            .Range(0, nbElements)
            .AsParallel()
            .ForAll(
                x =>
                {
                    var product = GenerateReturn(x);
                    concurrentData.TryAdd(product.ReturnId, product);
                });

        Data = concurrentData.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    private Return GenerateReturn(int index)
    {
        var parcel = parcelsGenerator.Data.ElementAt(_random.Next(parcelsGenerator.Data.Count)).Value;

        return new Return
        {
            ReturnId = new ReturnId($"RE_{index}_{GenerateHexadecimal(10)}"),
            ParcelId = parcel.ParcelId,
            OrderId = parcel.OrderId,
            ReturnStatus = (ReturnStatus)_random.Next(0, 2),
            Products = parcel.Products.Select(x => new ReturnProduct()
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
            }),
            CreationDate = DateTime.Now,
            UpdateDate = DateTime.Now
        };
    }
}