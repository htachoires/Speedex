using FluentValidation;
using Speedex.Domain.Commons;
using Speedex.Domain.Orders.Repositories;
using Speedex.Domain.Orders.Repositories.Dtos;
using Speedex.Domain.Products;
using Speedex.Domain.Products.Repositories;
using Speedex.Domain.Products.UseCases.CreateProduct;

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

        var createdOrder = command.ToOrder();
        var maxWeight = 30.0;
        var maxVolume = 1.0;
        var orderWeigth = 0.0;
        var orderVolume = 0.0;
        var productNotFoundError = new CreateOrderResult.ValidationError{Code = "Command_ProductNotFound_Error", Message = "Command product not found", PropertyName = "Product"};

        foreach (var product in createdOrder.Products)
        {
            var p = await _productRepository.GetProductById(product.ProductId, cancellationToken);
            var w = 0.0;
            var v = 0.0;
            if (p == null) return new CreateOrderResult { Success = false, Errors = [productNotFoundError]};
            w = p.Weight.Unit switch
            {
                WeightUnit.Mg => p.Weight.Value / 100_000,
                WeightUnit.Gr => p.Weight.Value / 1_000,
                WeightUnit.Kg => p.Weight.Value,
                _ => w
            };
            v = p.Dimensions.Unit switch
            {
                DimensionUnit.M => p.Dimensions.X * p.Dimensions.Y * p.Dimensions.Z,
                DimensionUnit.Cm => (p.Dimensions.X / 100) * (p.Dimensions.Y / 100) * (p.Dimensions.Z / 100),
                DimensionUnit.Mm => (p.Dimensions.X / 1_000) * (p.Dimensions.Y / 1_000) * (p.Dimensions.Z / 1_000),
                _ => v
            };
            orderWeigth += w;
            orderVolume += v;
        }
        
        var weightError = new CreateOrderResult.ValidationError{Code = "Command_WeightExceeded_Error", Message = "Command weight is more than 30kg", PropertyName = "Weight"};
        var volumeError = new CreateOrderResult.ValidationError{Code = "Command_VolumeExceeded_Error", Message = "Command volume is more than 1m3", PropertyName = "Volume"};
        
        if(orderWeigth > maxWeight) return new CreateOrderResult { Success = false, Errors = [weightError] };
        if(orderVolume > maxVolume) return new CreateOrderResult { Success = false, Errors = [volumeError] };
            
        var result = _orderRepository.UpsertOrder(createdOrder);

        if (result.Status != UpsertOrderResult.UpsertStatus.Success)
        {
            return new CreateOrderResult
            {
                Success = false,
                
            };
        }

        return new CreateOrderResult
        {
            OrderId = createdOrder.OrderId,
            Success = true,
        };
    }
}