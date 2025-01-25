using Microsoft.Extensions.Options;
using Speedex.Domain.Orders;
using Speedex.Domain.Orders.Repositories;
using Speedex.Domain.Parcels;
using Speedex.Domain.Parcels.Repositories;
using Speedex.Domain.Products;
using Speedex.Domain.Products.Repositories;
using Speedex.Domain.Returns;
using Speedex.Domain.Returns.Repositories;

namespace Speedex.Data;

public class DataGenerator(
    IDataGenerator<OrderId, Order> orderGenerator,
    IDataGenerator<ProductId, Product> productGenerator,
    IDataGenerator<ParcelId, Parcel> parcelGenerator,
    IDataGenerator<ReturnId, Return> returnGenerator,
    IProductRepository productRepository,
    IReturnRepository returnRepository,
    IOrderRepository orderRepository,
    IParcelRepository parcelRepository,
    IOptions<GenerateOptions> generateOptions)
    : IDataGenerator
{
    public void GenerateData()
    {
        var options = generateOptions.Value;

        productGenerator.GenerateData(options.NbProductElements);
        orderGenerator.GenerateData(options.NbOrderElements);
        parcelGenerator.GenerateData(options.NbParcelElements);
        returnGenerator.GenerateData(options.NbReturnElements);

        productGenerator.Data.Values
            .ToList()
            .ForEach(x => productRepository.UpsertProduct(x));

        orderGenerator.Data.Values
            .ToList()
            .ForEach(x => orderRepository.UpsertOrder(x));

        parcelGenerator.Data.Values
            .ToList()
            .ForEach(x => parcelRepository.UpsertParcel(x));

        returnGenerator.Data.Values
            .ToList()
            .ForEach(x => returnRepository.UpsertReturn(x));
    }
}