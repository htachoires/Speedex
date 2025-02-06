using FluentValidation;
using Speedex.Domain.Orders.Repositories;

namespace Speedex.Domain.Orders.Validators;

public class IsExistingOrderValidator : AbstractValidator<OrderId>
{
    public IsExistingOrderValidator(IOrderRepository orderRepository)
    {
        RuleFor(x => x)
            .MustAsync(async (orderId, cancellationToken) => await orderRepository.IsExistingOrder(orderId, cancellationToken))
            .WithErrorCode(nameof(IsExistingOrderValidator))
            .WithMessage("Order with id {PropertyValue} does not exist");
    }
}