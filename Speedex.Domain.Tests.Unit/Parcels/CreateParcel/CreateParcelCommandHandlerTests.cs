using Speedex.Domain.Parcels.UseCases.CreateParcel;
using FluentValidation;
using Speedex.Domain.Orders;
using Speedex.Domain.Orders.Repositories.Dtos;
using Speedex.Domain.Parcels;
using Speedex.Domain.Parcels.Repositories;
using Speedex.Domain.Products;
using Speedex.Domain.Products.Repositories;
using Speedex.Domain.Tests.Unit.Parcels;
using Speedex.Tests.Tools.TestDataBuilders.Domain.Orders.Commands;
using Speedex.Tests.Tools.TestDataBuilders.Domain.Parcels;

namespace Speedex.Domain.Tests.Unit.Parcels.CreateParcel;

public class CreateParcelCommandHandlerTests
{
    private readonly IValidator<CreateParcelCommand> _parcelValidator = Substitute.For<IValidator<CreateParcelCommand>>();

    public CreateParcelCommandHandlerTests()
    {
        _parcelValidator
            .ValidateAsync(Arg.Any<CreateParcelCommand>())
            .Returns(new FluentValidation.Results.ValidationResult());
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_Result_When_Parcel_Is_Created_With_Volume_Over_1_Cubic_Meter()
    {
        // Arrange
        var parcelRepository = Substitute.For<IParcelRepository>();
        var productRepository = Substitute.For<IProductRepository>();
        
        
        var product = new Product()
        {
            ProductId = new ProductId("productId"),
            Dimensions = new Dimensions
            {
                X = 1.1,
                Y = 1.1,
                Z = 1.1,
                Unit = DimensionUnit.M
            }
        };

        productRepository.GetProductById(new ProductId("productId"), CancellationToken.None).Returns(product);
        
        var handler = new CreateParcelCommandHandler(parcelRepository, productRepository, _parcelValidator);

        var parcelCommand = new CreateParcelCommand{
            OrderId = new OrderId("orderId"),
            Products = new List<CreateParcelCommand.ParcelProductCreateParcelCommand>
            {
                new()
                {
                    ProductId = product.ProductId,
                    Quantity = 1,
                }
            }
        };
        
        // Act
        var result = await handler.Handle(parcelCommand, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
    }

}