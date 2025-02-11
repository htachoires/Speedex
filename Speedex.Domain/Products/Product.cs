namespace Speedex.Domain.Products;

public record Product
{
    public ProductId ProductId { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string Category { get; init; }
    public string SecondLevelCategory { get; init; }
    public string ThirdLevelCategory { get; init; }
    public Price Price { get; init; }
    public Dimensions Dimensions { get; init; }
    public Weight Weight { get; init; }
    public DateTime CreationDate { get; init; }
    public DateTime UpdateDate { get; init; }
}

public record Price
{
    public decimal Amount { get; init; }
    public Currency Currency { get; init; }

    public Price ToEUR()
    {
        return Currency switch
        {
            Currency.EUR => this,
            Currency.RUB => new Price
            {
                Amount = Amount * 0.01m,
                Currency = Currency.EUR
            },
            Currency.USD => new Price
            {
                Amount = Amount * 0.98m,
                Currency = Currency.EUR
            }
        };
    }
}

public enum Currency
{
    EUR,
    USD,
    RUB
}

public record Weight
{
    public double Value { get; init; }
    public WeightUnit Unit { get; init; }

    public Weight ToKilograms()
    {
        return Unit switch
        {
            WeightUnit.Kg => this,
            WeightUnit.Gr => new Weight
            {
                Value = Value / 1_000,
                Unit = WeightUnit.Kg
            },
            WeightUnit.Mg => new Weight
            {
                Value = Value / 1_000_000,
                Unit = WeightUnit.Kg
            },
        };
    }
}

public enum WeightUnit
{
    Kg,
    Gr,
    Mg
}

public record ProductId(string Value);

public record Dimensions
{
    public double X { get; init; }
    public double Y { get; init; }
    public double Z { get; init; }
    public DimensionUnit Unit { get; init; }

    public double VolumeInCubicMeter
    {
        get
        {
            return Unit switch
            {
                DimensionUnit.Cm => X * Y * Z / 1_000_000,
                DimensionUnit.Mm => X * Y * Z / 1_000_000_000,
                DimensionUnit.M => X * Y * Z,
                _ => throw new ArgumentOutOfRangeException(nameof(Unit), Unit, "Dimension unit is not supported")
            };
        }
    }
}

public enum DimensionUnit
{
    M,
    Cm,
    Mm
}