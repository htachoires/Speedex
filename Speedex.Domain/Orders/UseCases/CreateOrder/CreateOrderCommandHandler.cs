using Speedex.Domain.Commons;
using Speedex.Domain.Orders.Repositories;
using Speedex.Domain.Orders.Repositories.Dtos;
using Speedex.Domain.Products;

namespace Speedex.Domain.Orders.UseCases.CreateOrder;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public CreateOrderResult Handle(CreateOrderCommand command)
    {
        var now = DateTime.Now;

        var newCommand = new Order
        {
            OrderId = new OrderId(Guid.NewGuid().ToString()),
            Products = command.Products.Select(x => new ProductCommand
            {
                ProductId = new ProductId(x.ProductId),
                Quantity = x.Quantity
            }),
            DeliveryType = DeliveryType.Standard,
            Recipient = new Recipient
            {
                FirstName = command.Recipient.FirstName,
                LastName = command.Recipient.LastName,
                Email = command.Recipient.Email,
                Phone = command.Recipient.Phone,
                Address = command.Recipient.Address,
                AdditionalAddress = command.Recipient.AdditionalAddress,
                City = command.Recipient.City,
                Country = command.Recipient.Country
            },
            CreationDate = now,
            UpdateDate = now
        };

        var result = _orderRepository.UpsertCommand(newCommand);

        if (result.Status != UpsertOrderResult.UpsertStatus.Success)
        {
            return new CreateOrderResult
            {
                Success = false
            };
        }

        return new CreateOrderResult
        {
            OrderId = newCommand.OrderId,
            Success = true,
        };
    }
}