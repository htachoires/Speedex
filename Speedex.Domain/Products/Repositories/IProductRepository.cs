using Speedex.Domain.Orders;
using Speedex.Domain.Products.Repositories.Dtos;

namespace Speedex.Domain.Products.Repositories;

public interface IProductRepository
{
    public UpsertProductResult UpsertProduct(Product product);
    public IEnumerable<Product> GetProducts(GetProductsDto query);
}