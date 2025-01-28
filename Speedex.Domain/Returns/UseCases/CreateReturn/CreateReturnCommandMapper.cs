namespace Speedex.Domain.Returns.UseCases.CreateReturn;

public static class CreateReturnCommandMapper
{
    public static Return ToReturn(this CreateReturnCommand command)
    {
        var now = DateTime.Now;

        var createdReturn = new Return
        {
            ReturnId = new ReturnId(Guid.NewGuid().ToString()),
            ReturnStatus = ReturnStatus.Created,
            OrderId = command.OrderId,
            Products = command.Products.Select(
                x => new ReturnProduct
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }),
            CreationDate = now,
            UpdateDate = now
        };
        return createdReturn;
    }
}