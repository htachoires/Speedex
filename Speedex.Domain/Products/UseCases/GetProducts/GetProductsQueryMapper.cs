using Speedex.Domain.Products.Repositories.Dtos;

namespace Speedex.Domain.Products.UseCases.GetProducts;

public static class GetProductsQueryMapper
{
    public static GetProductsDto ToGetProductsDto(this GetProductsQuery query)
    {
        const int defaultPageIndex = 1;
        const int defaultPageSize = 100;

        return new GetProductsDto
        {
            ProductId = query.ProductId,
            PageIndex = query.PageIndex ?? defaultPageIndex,
            PageSize = query.PageSize ?? defaultPageSize
        };
    }
}