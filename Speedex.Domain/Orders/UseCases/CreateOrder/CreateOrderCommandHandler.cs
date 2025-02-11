using FluentValidation;
using Speedex.Domain.Commons;
using Speedex.Domain.Orders.Repositories;
using Speedex.Domain.Orders.Repositories.Dtos;
using Speedex.Domain.Products.Repositories;

namespace Speedex.Domain.Orders.UseCases.CreateOrder;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IValidator<CreateOrderCommand> _commandValidator;

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

        //TODO: Check that weight in command is respected ( <30kg) for all containing products
        // hint use productRepository
        double totalWeight = 0;
        double totalVolume = 0.0;
        foreach (var commandProduct in command.Products)
        {
            var product = await _productRepository
                .GetProductById(commandProduct.ProductId, CancellationToken.None);
            totalWeight += product.Weight.ToKilograms().Value;
            totalVolume += product.Dimensions.VolumeInCubicMeter;
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

        if (totalVolume > 1.0)
        {
            return new CreateOrderResult
            {
                Success = false,
                Errors =
                [
                    new CreateOrderResult.ValidationError
                    {
                        Message = "Command volume is more than 1m\u00b3",
                        Code = "Command_Volume_Exceeded_Error"
                    }
                ]
            };
        }

        Order createdOrder = command.ToOrder();   
    
        var result = _orderRepository.UpsertOrder(createdOrder);

        if (result != null && result.Status != UpsertOrderResult.UpsertStatus.Success)
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