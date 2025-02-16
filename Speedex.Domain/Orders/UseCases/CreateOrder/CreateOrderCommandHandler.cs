using System.Diagnostics;
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

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IValidator<CreateOrderCommand> commandValidator, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _commandValidator = commandValidator;
        _productRepository = productRepository;
    }

    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken = default)
{
    // Validate command structure first
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
    
    var productIds = command.Products.Select(p => p.ProductId).ToList();
    
    double totalWeight = 0;
    foreach (var productId in productIds)
    {
        var product = await _productRepository.GetProductById(productId, cancellationToken);
        if (product != null)
        {
            totalWeight += product.Weight.ToKilograms().Value;
        }
    }
    
    if (totalWeight > 30)
    {
        return new CreateOrderResult
        {
            Success = false,
            Errors = new List<CreateOrderResult.ValidationError>
            {
                new CreateOrderResult.ValidationError
                {
                    Message = "Command weight is more than 30kg",
                    PropertyName = "Weight",
                    Code = "Command_WeightExceeded_Error"
                }
            }
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