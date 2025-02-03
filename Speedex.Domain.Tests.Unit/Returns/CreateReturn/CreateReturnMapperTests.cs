using Speedex.Domain.Orders;
using Speedex.Domain.Returns;
using Speedex.Domain.Returns.UseCases.CreateReturn;

namespace Speedex.Domain.Tests.Unit.Returns.CreateReturn;

public class CreateReturnMapperTests
{
    [Fact]
    public void ToReturn_Should_Create_Return_With_Return_Status_Created_When_Mapping_Create_Return_Command()
    {
        //Arrange
        CreateReturnCommand command = new CreateReturnCommand()
        {
            Products = [new CreateReturnCommand.ReturnProductCreateReturnCommand()]
        };

        //Act
        var createdReturn = command.ToReturn();

        //Assert
        Assert.Equal(ReturnStatus.Created, createdReturn.ReturnStatus);
    }

    [Fact]
    public void ToReturn_Should_Create_Return_With_Return_Id_Not_Null_When_Mapping_Create_Return_Command()
    {
        //Arrange
        CreateReturnCommand command = new CreateReturnCommand()
        {
            Products = [new CreateReturnCommand.ReturnProductCreateReturnCommand()]
        };

        //Act
        var createdReturn = command.ToReturn();

        //Assert
        Assert.NotNull(createdReturn.ReturnId);
    }

    [Fact]
    public void ToReturn_Should_Create_Return_With_Order_Id_Not_Null_When_Mapping_Create_Return_Command()
    {
        //Arrange
        CreateReturnCommand command = new CreateReturnCommand()
        {
            OrderId = new OrderId("OrderId"),
            Products = [new CreateReturnCommand.ReturnProductCreateReturnCommand()]
        };

        //Act
        var createdReturn = command.ToReturn();

        //Assert
        Assert.NotNull(createdReturn.OrderId);
        Assert.Equal(new OrderId("OrderId"), createdReturn.OrderId);
    }
}