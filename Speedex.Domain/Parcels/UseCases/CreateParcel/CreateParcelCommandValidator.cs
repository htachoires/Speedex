using FluentValidation;
using Speedex.Domain.Orders;
using Speedex.Domain.Products;

namespace Speedex.Domain.Parcels.UseCases.CreateParcel;

public class CreateParcelCommandValidator : AbstractValidator<CreateParcelCommand>
{
    public CreateParcelCommandValidator(IValidator<ProductId> productValidator, IValidator<OrderId> orderValidator)
    {
        RuleFor(x => x.Products.Select(p => p.ProductId))
            .ForEach(x => x.SetValidator(productValidator));

        RuleFor(x => x.OrderId)
            .SetValidator(orderValidator);
    }
}