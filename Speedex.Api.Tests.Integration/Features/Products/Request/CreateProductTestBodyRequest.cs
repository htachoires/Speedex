namespace Speedex.Api.Tests.Integration.Features.Products.Request;

public class CreateProductTestBodyRequest
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? Category { get; init; }
    public string? SecondLevelCategory { get; init; }
    public string? ThirdLevelCategory { get; init; }
    public PriceGetProductTestBodyRequest? Price { get; init; }
    public DimensionsGetProductTestBodyRequest? Dimensions { get; init; }
    public WeightGetProductTestBodyRequest? Weight { get; init; }

    public record PriceGetProductTestBodyRequest
    {
        public decimal? Amount { get; init; }
        public string? Currency { get; init; }
    }

    public record WeightGetProductTestBodyRequest
    {
        public double? Value { get; init; }
        public string? Unit { get; init; }
    }

    public record DimensionsGetProductTestBodyRequest
    {
        public double? X { get; init; }
        public double? Y { get; init; }
        public double? Z { get; init; }
        public string? Unit { get; init; }
    }
}