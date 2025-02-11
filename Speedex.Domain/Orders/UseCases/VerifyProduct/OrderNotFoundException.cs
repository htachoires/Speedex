namespace Speedex.Domain.Orders.UseCases.VerifyProduct;

public class OrderNotFoundException : Exception
{
    public OrderNotFoundException(OrderId orderId)
        : base($"La commande {orderId} est introuvable.")
    {
    }
}