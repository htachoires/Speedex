using Speedex.Api.Features.Products.Responses;
using Speedex.Domain.Products.UseCases.GetProducts;

namespace Speedex.Api.Features.Products.Mappers;

public static class GetProductsResponseMapper
{
    public static GetProductsResponse ToResponse(this GetProductsQueryResult result)
    {
        return new GetProductsResponse
        {
            Items = result.Items.Select(x => new GetProductsResponse.GetProductItemResponse
            {
                ProductId = x.ProductId.Value,
                Name = x.Name,
                Description = x.Description,
                Category = x.Category,
                SecondLevelCategory = x.SecondLevelCategory,
                ThirdLevelCategory = x.ThirdLevelCategory,
                Price = new GetProductsResponse.GetProductItemResponse.PriceGetProductItemResponse()
                {
                    Amount = x.Price.Amount,
                    Currency = x.Price.Currency.ToString(),
                },
                Dimensions = new GetProductsResponse.GetProductItemResponse.DimensionsGetProductItemResponse
                {
                    X = x.Dimensions.X,
                    Y = x.Dimensions.Y,
                    Z = x.Dimensions.Z,
                    Unit = x.Dimensions.Unit.ToString()
                },
                Weight = new GetProductsResponse.GetProductItemResponse.WeightGetProductItemResponse
                {
                    Value = x.Weight.Value,
                    Unit = x.Weight.Unit.ToString()
                },
                CreationDate = x.CreationDate.ToString("u"),
                UpdateDate = x.UpdateDate.ToString("u"),
            })
        };
    }
}