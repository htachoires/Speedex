using Speedex.Api.Features.Products.Requests;
using Speedex.Domain.Products;
using Speedex.Domain.Products.UseCases.GetProducts;

namespace Speedex.Api.Features.Products.Mappers;

public static class GetProductsQueryParamsMapper
{
    public static GetProductsQuery ToQuery(this GetProductsQueryParams queryParams)
    {
        return new GetProductsQuery
        {
            ProductId = queryParams.ProductId is not null ? new ProductId(queryParams.ProductId) : null,
            PageIndex = queryParams.PageIndex,
            PageSize = queryParams.PageSize,
            Category = queryParams.Category,
        };
    }
}