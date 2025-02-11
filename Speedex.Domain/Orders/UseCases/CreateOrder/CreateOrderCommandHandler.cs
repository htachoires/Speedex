using FluentValidation;
using Speedex.Domain.Commons;
using Speedex.Domain.Orders.Repositories;
using Speedex.Domain.Orders.Repositories.Dtos;
using Speedex.Domain.Products.Repositories;

namespace Speedex.Domain.Orders.UseCases.CreateOrder;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IValidator<CreateOrderCommand> _commandValidator;
    private readonly IProductRepository _productRepository;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IValidator<CreateOrderCommand> commandValidator,
        IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _commandValidator = commandValidator;
        _productRepository = productRepository;
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
        double totalWeight = 0;
        foreach (var commandProduct in command.Products)
        {
            var product = await _productRepository
                .GetProductById(commandProduct.ProductId, CancellationToken.None);
            totalWeight += product.Weight.ToKilograms().Value;
        }

        if (totalWeight > 30)
        {
            return new CreateOrderResult
            {
                Success = false,
                Errors =
                [
                    new CreateOrderResult.ValidationError
                    {
                        Message = "Command weight is more than 30kg",
                        Code = "Command_WeightExceeded_Error"
                    }
                ]
            };
        }

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