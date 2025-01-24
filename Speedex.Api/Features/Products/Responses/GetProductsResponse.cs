namespace Speedex.Api.Features.Products.Responses;

public class GetProductsResponse
{
    public IEnumerable<GetProductItemResponse> Items { get; init; }

    public record GetProductItemResponse
    {
        public string ProductId { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public string Category { get; init; }
        public string SecondLevelCategory { get; init; }
        public string ThirdLevelCategory { get; init; }
        public PriceGetProductItemResponse Price { get; init; }
        public DimensionsGetProductItemResponse Dimensions { get; init; }
        public WeightGetProductItemResponse Weight { get; init; }
        public string CreationDate { get; init; }
        public string UpdateDate { get; init; }

        public record PriceGetProductItemResponse
        {
            public decimal Amount { get; init; }
            public string Currency { get; init; }
        }

        public record WeightGetProductItemResponse
        {
            public double Value { get; init; }
            public string Unit { get; init; }
        }

        public record DimensionsGetProductItemResponse
        {
            public double X { get; init; }
            public double Y { get; init; }
            public double Z { get; init; }
            public string Unit { get; init; }
        }
    }
}