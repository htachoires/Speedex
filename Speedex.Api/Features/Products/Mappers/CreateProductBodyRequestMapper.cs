using Speedex.Api.Features.Products.Requests;
using Speedex.Domain.Products;
using Speedex.Domain.Products.UseCases.CreateProduct;

namespace Speedex.Api.Features.Products.Mappers;

public static class CreateProductBodyRequestMapper
{
    public static CreateProductCommand ToCommand(this CreateProductBodyRequest bodyRequest)
    {
        return new CreateProductCommand
        {
            Name = bodyRequest.Name,
            Description = bodyRequest.Description,
            Category = bodyRequest.Category,
            SecondLevelCategory = bodyRequest.SecondLevelCategory,
            ThirdLevelCategory = bodyRequest.ThirdLevelCategory,
            Price = new CreateProductCommand.PriceCreateProductCommand
            {
                Amount = bodyRequest.Price.Amount.Value,
                Currency = Enum.Parse<Currency>(bodyRequest.Price.Currency, false),
            },
            Dimensions = new CreateProductCommand.DimensionsCreateProductCommand
            {
                X = bodyRequest.Dimensions.X.Value,
                Y = bodyRequest.Dimensions.Y.Value,
                Z = bodyRequest.Dimensions.Z.Value,
                Unit = Enum.Parse<DimensionUnit>(bodyRequest.Dimensions.Unit, true),
            },
            Weight = new CreateProductCommand.WeightCreateProductCommand
            {
                Value = bodyRequest.Weight.Value.Value,
                Unit = Enum.Parse<WeightUnit>(bodyRequest.Weight.Unit, true),
            }
        };
    }
}