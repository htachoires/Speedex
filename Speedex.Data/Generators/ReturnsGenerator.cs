using Speedex.Domain.Parcels;
using Speedex.Domain.Returns;

namespace Speedex.Data.Generators;

public class ReturnsGenerator(IDataGenerator<ParcelId, Parcel> parcelsGenerator) : IDataGenerator<ReturnId, Return>
{
    public Dictionary<ReturnId, Return> Data { get; private set; }
    private readonly Random _random = new();

    public void GenerateData(int nbElements)
    {
        Data = Enumerable
            .Range(0, nbElements)
            .Select(_ => GenerateReturn())
            .ToDictionary(x => x.ReturnId);
    }

    private Return GenerateReturn()
    {
        var parcel = parcelsGenerator.Data.ElementAt(_random.Next(parcelsGenerator.Data.Count)).Value;

        return new Return
        {
            ReturnId = new ReturnId(Guid.NewGuid().ToString()),
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