using System.ComponentModel;

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
    public Dimensions Dimensions { get; init; } = new Dimensions { X = 0, Y = 0, Z = 0, Unit = DimensionUnit.M };
    
    public Weight Weight { get; init; } = new Weight { Value = 0, Unit = WeightUnit.Kg };
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