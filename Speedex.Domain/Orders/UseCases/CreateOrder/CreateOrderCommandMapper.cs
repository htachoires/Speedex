namespace Speedex.Domain.Orders.UseCases.CreateOrder;

public static class CreateOrderCommandMapper
{
    public static Order ToOrder(this CreateOrderCommand command)
    {
        var now = DateTime.Now;

        //Tip: we can see stored variable in the debug console
        var orderId = new OrderId(Guid.NewGuid().ToString());

        return new Order
        {
            OrderId = orderId,
            Products = command.Products.Select(
                x => new OrderProduct
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }),
            DeliveryType = command.DeliveryType,
            Recipient = new Recipient
            {
                FirstName = command.Recipient.FirstName,
                LastName = command.Recipient.LastName.ToUpper(),
                Email = command.Recipient.Email,
                PhoneNumber = command.Recipient.Phone,
                Address = command.Recipient.Address,
                AdditionalAddress = command.Recipient.AdditionalAddress,
                City = command.Recipient.City,
                Country = command.Recipient.Country
            },
            CreationDate = now,
            UpdateDate = now
        };
    }
}