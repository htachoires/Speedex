using Speedex.Domain.Orders;
using Speedex.Domain.Orders.Repositories;
using Speedex.Domain.Products;
using System;
using System.Threading.Tasks;

namespace Speedex.Domain.Orders.UseCases.VerifyProduct
{
    public class VerifyProductInOrderHandler
    {
        private readonly IOrderRepository _orderRepository;

        public VerifyProductInOrderHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public async Task VerifyProduct(OrderId orderId, ProductId productId)
        {
            // Récupération de la commande
            var order = await _orderRepository.GetOrderById(orderId);
            if (order == null)
            {
                throw new OrderNotFoundException(orderId);
            }

            // Vérification si le produit est dans la commande
            bool productExists = order.Products.Any(p => p.ProductId == productId);
            if (!productExists)
            {
                throw new ProductNotInOrderException(orderId, productId);
            }
        }
    }
}