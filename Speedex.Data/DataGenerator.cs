using Speedex.Domain.Orders;
using Speedex.Domain.Orders.Repositories;
using Speedex.Domain.Products;
using Speedex.Domain.Products.Repositories;

namespace Speedex.Data;

public class DataGenerator : IDataGenerator
{
    private readonly IDataGenerator<OrderId, Order> _orderGenerator;
    private readonly IDataGenerator<ProductId, Product> _productGenerator;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;

    public DataGenerator(
        IDataGenerator<OrderId, Order> orderGenerator,
        IDataGenerator<ProductId, Product> productGenerator,
        IProductRepository productRepository,
        IOrderRepository orderRepository
    )
    {
        _orderGenerator = orderGenerator;
        _productGenerator = productGenerator;
        _productRepository = productRepository;
        _orderRepository = orderRepository;
    }

    public void GenerateData()
    {
        _productGenerator.GenerateData(10_000);
        _orderGenerator.GenerateData(500);

        _productGenerator.Data.Values
            .ToList()
            .ForEach(x => _productRepository.UpsertProduct(x));

        _orderGenerator.Data.Values
            .ToList()
            .ForEach(x => _orderRepository.UpsertOrder(x));
    }
}