using Speedex.Domain.Commons;
using Speedex.Domain.Products.Repositories;

namespace Speedex.Domain.Products.UseCases.GetProducts;

public class GetProductsQueryHandler(IProductRepository productRepository)
    : IQueryHandler<GetProductsQuery, GetProductsQueryResult>
{
    public Task<GetProductsQueryResult> Query(GetProductsQuery query, CancellationToken cancellationToken = default)
    {
        var result = productRepository.GetProducts(query.ToGetProductsDto());

        return Task.FromResult(new GetProductsQueryResult { Items = result });
    }
}