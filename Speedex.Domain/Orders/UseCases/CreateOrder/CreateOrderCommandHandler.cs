using FluentValidation;
using Speedex.Domain.Commons;
using Speedex.Domain.Orders.Repositories;
using Speedex.Domain.Orders.Repositories.Dtos;

namespace Speedex.Domain.Orders.UseCases.CreateOrder;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IValidator<CreateOrderCommand> _commandValidator;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IValidator<CreateOrderCommand> commandValidator)
    {
        _orderRepository = orderRepository;
        _commandValidator = commandValidator;
    }

    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _commandValidator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new CreateOrderResult
            {
                Success = false,
                Errors = validationResult.Errors.Select(
                    x => new CreateOrderResult.ValidationError
                    {
                        Message = x.ErrorMessage,
                        PropertyName = x.PropertyName,
                        Code = x.ErrorCode,
                    }).ToList()
            };
        }
        
        const decimal maxAllowedVolume = 1.0m; // 1m³ max
        decimal totalVolume = command.Products
            .Sum(p => (p.Length * p.Width * p.Height) * p.Quantity);

        if (totalVolume > maxAllowedVolume)
        {
            return new CreateOrderResult
            {
                Success = false,
                Errors = new List<CreateOrderResult.ValidationError>
                {
                    new CreateOrderResult.ValidationError
                    {
                        Message = "Total volume exceeds limit",
                        PropertyName = "Products",
                        Code = "VolumeExceeded"
                    }
                }
            };
        }

        //TODO: Check that weight in command is respected ( <30kg) for all containing products
        // hint use productRepository
        var createdOrder = command.ToOrder();

        var result = _orderRepository.UpsertOrder(createdOrder);

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