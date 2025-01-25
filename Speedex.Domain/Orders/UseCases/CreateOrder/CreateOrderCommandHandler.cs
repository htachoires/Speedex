using Speedex.Domain.Commons;
using Speedex.Domain.Orders.Repositories;
using Speedex.Domain.Orders.Repositories.Dtos;

namespace Speedex.Domain.Orders.UseCases.CreateOrder;

public class CreateOrderCommandHandler(IOrderRepository orderRepository)
    : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public CreateOrderResult Handle(CreateOrderCommand command)
    {
        var createdOrder = command.ToOrder();

        var result = orderRepository.UpsertOrder(createdOrder);

        if (result.Status != UpsertOrderResult.UpsertStatus.Success)
        {
            return new CreateOrderResult
            {
                Success = false
            };
        }

        return new CreateOrderResult
        {
            OrderId = createdOrder.OrderId,
            Success = true,
        };
    }
}