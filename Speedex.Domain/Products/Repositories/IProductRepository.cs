using Speedex.Domain.Products.Repositories.Dtos;

namespace Speedex.Domain.Products.Repositories;

public interface IProductRepository
{
    UpsertProductResult UpsertProduct(Product product);

    IEnumerable<Product> GetProducts(GetProductsDto query);

    Task<bool> IsExistingProduct(ProductId productId, CancellationToken cancellationToken);
}