using Speedex.Domain.Orders;
using Speedex.Domain.Returns.UseCases.CreateReturn;

namespace Speedex.Domain.Tests.Unit.Returns.CreateReturn;

public class CreateReturnMapperWithTheoryTests
{
    [Theory]
    [InlineData("fooOrderId", "fooOrderId")]
    [InlineData("fooOrderId2", "fooOrderId2")]
    [InlineData("fooOrderId3", "fooOrderId3")]
    public void ToReturn_Should_Create_Return_With_Specific_OrderId_When_Mapping_Create_Return_Command(
        string orderId,
        string expectedOrderId)
    {
        //Arrange
        var command = new CreateReturnCommand()
        {
            OrderId = new OrderId(orderId),
            Products = [new CreateReturnCommand.ReturnProductCreateReturnCommand()]
        };

        //Act
        var createdReturn = command.ToReturn();

        //Assert
        Assert.Equal(expectedOrderId, createdReturn.OrderId.Value);
    }

    // [Theory]
    public void ToReturn_Should_Create_Return_With_Specific_ParcelId_When_Mapping_Create_Return_Command()
    {
        //Arrange
        //TODO: Instantiate CreateReturnCommand, used for your test (in act part)

        //Act
        // var createdReturn = command.ToReturn();

        //Assert
        //TODO: add the appropriate assertion to check return status
        //Assert.Equal ?
        //Assert.Null ?
        //Assert.NotNull ?
    }

    // [Theory]
    public void ToReturn_Should_Create_Return_With_Specific_ProductId_When_Mapping_Create_Return_Command()
    {
        //Arrange
        //TODO: Instantiate CreateReturnCommand, used for your test (in act part)

        //Act
        // var createdReturn = command.ToReturn();

        //Assert
        //TODO: add the appropriate assertion to check return status
        //Assert.Equal ?
        //Assert.Null ?
        //Assert.NotNull ?
    }
}