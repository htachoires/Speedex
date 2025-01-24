using Speedex.Domain.Products.Repositories.Dtos;

namespace Speedex.Domain.Products.UseCases.GetProducts;

public static class GetProductsQueryMapper
{
    public static GetProductsDto ToGetProductsDto(this GetProductsQuery query)
    {
        return new GetProductsDto
        {
            ProductId = query.ProductId,
            PageIndex = query.PageIndex,
            PageSize = query.PageSize
        };
    }
}