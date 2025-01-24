using Speedex.Domain.Commons;
using Speedex.Domain.Products.Repositories;
using Speedex.Domain.Products.Repositories.Dtos;

namespace Speedex.Domain.Products.UseCases.CreateProduct;

public class CreateProductCommandHandler(IProductRepository productRepository)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public CreateProductResult Handle(CreateProductCommand command)
    {
        var now = DateTime.Now;

        var newCommand = new Product
        {
            ProductId = new ProductId(Guid.NewGuid().ToString()),
            Name = command.Name,
            Description = command.Description,
            Category = command.Category,
            SecondLevelCategory = command.SecondLevelCategory,
            ThirdLevelCategory = command.ThirdLevelCategory,
            Price = new Price
            {
                Amount = command.Price.Amount,
                Currency = command.Price.Currency,
            },
            Dimensions = new Dimensions
            {
                X = command.Dimensions.X,
                Y = command.Dimensions.Y,
                Z = command.Dimensions.Z,
                Unit = command.Dimensions.Unit,
            },
            Weight = new Weight
            {
                Value = command.Weight.Value,
                Unit = command.Weight.Unit,
            },
            CreationDate = now,
            UpdateDate = now,
        };

        var result = productRepository.UpsertProduct(newCommand);

        if (result.Status != UpsertProductResult.UpsertStatus.Success)
        {
            return new CreateProductResult
            {
                Success = false
            };
        }

        return new CreateProductResult
        {
            ProductId = newCommand.ProductId,
            Success = true,
        };
    }
}