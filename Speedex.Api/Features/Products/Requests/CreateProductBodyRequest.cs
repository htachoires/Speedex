namespace Speedex.Api.Features.Products.Requests;

public record CreateProductBodyRequest
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? Category { get; init; }
    public string? SecondLevelCategory { get; init; }
    public string? ThirdLevelCategory { get; init; }
    public PriceGetProductBodyRequest? Price { get; init; }
    public DimensionsGetProductBodyRequest? Dimensions { get; init; }
    public WeightGetProductBodyRequest? Weight { get; init; }

    public record PriceGetProductBodyRequest
    {
        public decimal? Amount { get; init; }
        public string? Currency { get; init; }
    }

    public record WeightGetProductBodyRequest
    {
        public double? Value { get; init; }
        public string? Unit { get; init; }
    }

    public record DimensionsGetProductBodyRequest
    {
        public double? X { get; init; }
        public double? Y { get; init; }
        public double? Z { get; init; }
        public string? Unit { get; init; }
    }
}