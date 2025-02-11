using Speedex.Domain.Orders.Repositories;
using Speedex.Domain.Products;

namespace Speedex.Domain.Orders.UseCases.VerifyProduct;

public class VerifyProductQuantityHandler
{
    private readonly IOrderRepository _orderRepository;

    public VerifyProductQuantityHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task VerifyProductQuantity(OrderId orderId, ProductId productId, int quantityToAdd)
    {
        var order = await _orderRepository.GetOrderById(orderId);
        
        if (order == null)
        {
            throw new OrderNotFoundException(orderId);
        }

        var productInOrder = order.Products.FirstOrDefault(p => p.ProductId == productId);

        if (productInOrder == null)
        {
            throw new ProductNotInOrderException(orderId, productId);
        }

        if (quantityToAdd > productInOrder.Quantity) // 🔥 Vérification de la quantité
        {
            throw new ProductQuantityExceededException(productId, quantityToAdd, productInOrder.Quantity);
        }
    }
}