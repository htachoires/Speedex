using Speedex.Domain.Orders;
using Speedex.Domain.Orders.Repositories;
using Speedex.Domain.Parcels;
using Speedex.Domain.Parcels.Repositories;
using Speedex.Domain.Products;
using Speedex.Domain.Products.Repositories;
using Speedex.Domain.Returns;
using Speedex.Domain.Returns.Repositories;

namespace Speedex.Data;

public class DataGenerator : IDataGenerator
{
    private readonly IDataGenerator<OrderId, Order> _orderGenerator;
    private readonly IDataGenerator<ProductId, Product> _productGenerator;
    private readonly IDataGenerator<ParcelId, Parcel> _parcelGenerator;
    private readonly IDataGenerator<ReturnId, Return> _returnGenerator;
    private readonly IProductRepository _productRepository;
    private readonly IReturnRepository _returnRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IParcelRepository _parcelRepository;

    public DataGenerator(
        IDataGenerator<OrderId, Order> orderGenerator,
        IDataGenerator<ProductId, Product> productGenerator,
        IDataGenerator<ParcelId, Parcel> parcelGenerator,
        IDataGenerator<ReturnId, Return> returnGenerator,
        IProductRepository productRepository,
        IReturnRepository returnRepository,
        IOrderRepository orderRepository,
        IParcelRepository parcelRepository)
    {
        _orderGenerator = orderGenerator;
        _productGenerator = productGenerator;
        _parcelGenerator = parcelGenerator;
        _returnGenerator = returnGenerator;
        _productRepository = productRepository;
        _returnRepository = returnRepository;
        _orderRepository = orderRepository;
        _parcelRepository = parcelRepository;
    }

    public void GenerateData()
    {
        _productGenerator.GenerateData(10_000);
        _orderGenerator.GenerateData(500);
        _parcelGenerator.GenerateData(500);
        _returnGenerator.GenerateData(250);

        _productGenerator.Data.Values
            .ToList()
            .ForEach(x => _productRepository.UpsertProduct(x));

        _orderGenerator.Data.Values
            .ToList()
            .ForEach(x => _orderRepository.UpsertOrder(x));

        _parcelGenerator.Data.Values
            .ToList()
            .ForEach(x => _parcelRepository.UpsertParcel(x));

        _returnGenerator.Data.Values
            .ToList()
            .ForEach(x => _returnRepository.UpsertReturn(x));
    }
}