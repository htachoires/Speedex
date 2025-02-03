using FluentValidation;
using FluentValidation.Results;
using Speedex.Domain.Orders;
using Speedex.Domain.Parcels;
using Speedex.Domain.Returns;
using Speedex.Domain.Returns.Repositories;
using Speedex.Domain.Returns.Repositories.Dtos;
using Speedex.Domain.Returns.UseCases.CreateReturn;

namespace Speedex.Domain.Tests.Unit.Returns.CreateReturn;

public class CreateReturnTests
{
    [Fact]
    public async Task Handle_Should_Return_Success_Result_When_Return_Is_Created_Successfully()
    {
        // Arrange
        var orderRepository = Substitute.For<IReturnRepository>();
        var commandValidator = Substitute.For<IValidator<CreateReturnCommand>>();

        commandValidator
            .ValidateAsync(Arg.Any<CreateReturnCommand>())
            .Returns(new ValidationResult());

        orderRepository
            .UpsertReturn(Arg.Any<Return>())
            .Returns(new UpsertReturnResult { Status = UpsertReturnResult.UpsertStatus.Success });

        var command = new CreateReturnCommand
        {
            ParcelId = new ParcelId("ParcelId"),
            OrderId = new OrderId("OrderId"),
            Products = [
                new CreateReturnCommand.ReturnProductCreateReturnCommand()
            ]
        };

        var handler = new CreateReturnCommandHandler(orderRepository, commandValidator);

        // Act
        var result = await handler.Handle(command);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.ReturnId);
    }
}