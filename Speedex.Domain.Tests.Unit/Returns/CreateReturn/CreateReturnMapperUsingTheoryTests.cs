using Speedex.Domain.Orders;
using Speedex.Domain.Returns.UseCases.CreateReturn;

namespace Speedex.Domain.Tests.Unit.Returns.CreateReturn;

public class CreateReturnMapperUsingTheoryTests
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
}