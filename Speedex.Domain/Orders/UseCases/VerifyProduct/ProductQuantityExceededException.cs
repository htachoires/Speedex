using Speedex.Domain.Products;

namespace Speedex.Domain.Orders.UseCases.VerifyProduct;


public class ProductQuantityExceededException : Exception
{
    public ProductQuantityExceededException(ProductId productId, int maxAllowed, int attempted)
        : base($"Le produit {productId} ne peut pas être ajouté avec {attempted} unités (max autorisé: {maxAllowed}).")
    {
    }
}