using Speedex.Domain.Commons;
using Speedex.Domain.Products.Repositories;

namespace Speedex.Domain.Products.UseCases.GetProducts;

public class GetProductsQueryHandler(IProductRepository productRepository)
    : IQueryHandler<GetProductsQuery, GetProductsQueryResult>
{
    public GetProductsQueryResult Query(GetProductsQuery query)
    {
        var result = productRepository.GetProducts(query.ToGetProductsDto());

        return new GetProductsQueryResult
        {
            Items = result
        };
    }
}