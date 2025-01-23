using Speedex.Domain.Orders;
using Speedex.Domain.Parcels;
using Speedex.Domain.Returns;

namespace Speedex.Data.Generators;

public class ReturnsGenerator : IDataGenerator<ReturnId, Return>
{
    private readonly IDataGenerator<ParcelId, Parcel> _parcelsGenerator;
    public Dictionary<ReturnId, Return> Data { get; private set; }
    private readonly Random _random;

    public ReturnsGenerator(IDataGenerator<ParcelId, Parcel> parcelsGenerator)
    {
        _parcelsGenerator = parcelsGenerator;
        _random = new Random();
    }

    public void GenerateData(int nbElements)
    {
        Data = Enumerable
            .Range(0, nbElements)
            .Select(_ => GenerateReturn())
            .ToDictionary(x => x.ReturnId);
    }

    private Return GenerateReturn()
    {
        var parcel = _parcelsGenerator.Data.ElementAt(_random.Next(_parcelsGenerator.Data.Count)).Value;

        return new Return
        {
            ReturnId = new ReturnId(Guid.NewGuid().ToString()),
            ParcelId = parcel.ParcelId,
            OrderId = parcel.OrderId,
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