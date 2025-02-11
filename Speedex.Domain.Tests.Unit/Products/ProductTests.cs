using FluentValidation;
using Speedex.Domain.Orders;
using Speedex.Domain.Orders.Repositories;
using Speedex.Domain.Orders.UseCases.CreateOrder;
using Speedex.Domain.Parcels;
using Speedex.Domain.Parcels.Repositories;
using Speedex.Domain.Parcels.Repositories.Dtos;
using Speedex.Domain.Parcels.UseCases.CreateParcel;
using Speedex.Domain.Products;
using Speedex.Tests.Tools.TestDataBuilders.Domain.Parcels;

namespace Speedex.Domain.Tests.Unit.Products;

public class ProductTests
{
    
    private readonly IValidator<CreateParcelCommand> _validator = Substitute.For<IValidator<CreateParcelCommand>>();
    [Theory]
    [InlineData(0.5, 0.5, 0.5, DimensionUnit.M, 0.125)]
    [InlineData(1, 1, 1, DimensionUnit.M, 1)]
    [InlineData(50, 50, 50, DimensionUnit.Cm, 0.125)]
    [InlineData(100, 100, 100, DimensionUnit.Cm, 1)]
    [InlineData(500, 500, 500, DimensionUnit.Mm, 0.125)]
    [InlineData(1_000, 1_000, 1_000, DimensionUnit.Mm, 1)]
    public void CreateProduct_Should_Return_Volume_In_CubicMeter(double x, double y, double z, DimensionUnit unit, double expectedVolume)
    {
        //Arrange
        var product = new Product
        {
            Dimensions = new Dimensions
            {
                X = x,
                Y = y,
                Z = z,
                Unit = unit
            }
        };

        //Act
        var volume = product.Dimensions.VolumeInCubicMeter;

        //Assert
        Assert.Equal(expectedVolume, volume);
    }
    
[Fact]
public async Task CreateParcel_Should_Return_Error_When_Product_Not_Found_In_Order()
{
    // Arrange
    var parcelRepository = Substitute.For<IParcelRepository>();
    var orderRepository = Substitute.For<IOrderRepository>();
    var validator = Substitute.For<IValidator<CreateParcelCommand>>();
    
    var order = new Order
    {
        OrderId = new OrderId("order1"),
        Products = new List<OrderProduct>
        {
            new OrderProduct { ProductId = new ProductId("product1"), Quantity = 5 }
        }
    };
    
    orderRepository.GetOrderById(Arg.Any<OrderId>(), Arg.Any<CancellationToken>()).Returns(order);
    
    parcelRepository.UpsertParcel(Arg.Any<Parcel>()).Returns(new UpsertParcelResult { Status = UpsertParcelResult.UpsertStatus.Success });

    validator.ValidateAsync(Arg.Any<CreateParcelCommand>(), Arg.Any<CancellationToken>())
        .Returns(new FluentValidation.Results.ValidationResult());
    
    var parcel = new CreateParcelCommand
    {
        OrderId = new OrderId("order1"),
        Products = new List<CreateParcelCommand.ParcelProductCreateParcelCommand>
        {
            new CreateParcelCommand.ParcelProductCreateParcelCommand
                { ProductId = new ProductId("product2"), Quantity = 1 }
        }
    };

    var handler = new CreateParcelCommandHandler(parcelRepository, validator, orderRepository);

    // Act
    var result = await handler.Handle(parcel);

    // Assert
    Assert.False(result.Success);
    Assert.Contains(result.Errors, e => e.Code == "Product_Not_Found");
}



}