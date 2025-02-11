using System;
using Speedex.Domain.Products;

namespace Speedex.Domain.Orders.UseCases.VerifyProduct
{
    public class ProductNotInOrderException : Exception
    {
        public ProductNotInOrderException(OrderId orderId, ProductId productId)
            : base($"Le produit {productId} n'est pas dans la commande {orderId}.")
        {
        }
    }
}