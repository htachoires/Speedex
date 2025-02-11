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

        decimal totalWeight = 0;
        decimal totalPrice = 0;
        foreach (var product in command.Products)
        {
            var productData = await _productRepository.GetProductById(product.ProductId, cancellationToken);
            if (productData != null)
            {
                 totalWeight += (decimal)productData.Weight.Value * product.Quantity; 
                 totalPrice += productData.Price.ToEUR().Amount * product.Quantity;
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
                        Code = "Command_WeightExceeded_Error"
                    }
                },
                totalPrice = totalPrice,
            };
        }
        
        var totalVolume=0;
        foreach (var product in command.Products)
        {
            var productData = await _productRepository.GetProductById(product.ProductId, cancellationToken);
            if (productData != null)
            {
                totalVolume += (int)productData.Dimensions.VolumeInCubicMeter * product.Quantity;
            }
        }
        if(totalVolume>1)
        {
            return new CreateOrderResult
            {
                Success = false,
                Errors = new List<CreateOrderResult.ValidationError>
                {
                    new CreateOrderResult.ValidationError
                    {
                        Message = "Command volume is more than 1 cubic meter",
                        Code = "Command_VolumeExceeded_Error"
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
                Success = false,
                totalPrice = totalPrice,
            };
        }

        return new CreateOrderResult
        {
            OrderId = createdOrder.OrderId,
            Success = true,
            totalPrice = totalPrice,
        };
    }

}