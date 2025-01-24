using Speedex.Domain.Commons;

namespace Speedex.Domain.Products.UseCases.CreateProduct;

public record CreateProductCommand : ICommand
{
    public string Name { get; init; }
    public string Description { get; init; }
    public string Category { get; init; }
    public string SecondLevelCategory { get; init; }
    public string ThirdLevelCategory { get; init; }
    public PriceCreateProductCommand Price { get; init; }
    public DimensionsCreateProductCommand Dimensions { get; init; }
    public WeightCreateProductCommand Weight { get; init; }

    public record PriceCreateProductCommand
    {
        public decimal Amount { get; init; }
        public Currency Currency { get; init; }
    }

    public record WeightCreateProductCommand
    {
        public double Value { get; init; }
        public WeightUnit Unit { get; init; }
    }

    public record DimensionsCreateProductCommand
    {
        public double X { get; init; }
        public double Y { get; init; }
        public double Z { get; init; }
        public DimensionUnit Unit { get; init; }
    }
}