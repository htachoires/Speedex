using Speedex.Domain.Products;

namespace Speedex.Domain.Tests.Unit.Products;

public class ProductTests
{
    [Theory]
    [InlineData(0.5, 0.5, 0.5, DimensionUnit.M, 0.125)]
    [InlineData(1, 1, 1, DimensionUnit.M, 1)]
    [InlineData(50, 50, 50, DimensionUnit.Cm, 0.125)]
    [InlineData(100, 100, 100, DimensionUnit.Cm, 1)]
    [InlineData(500, 500, 500, DimensionUnit.Mm, 0.125)]
    [InlineData(1_000, 1_000, 1_000, DimensionUnit.Mm, 1)]
    public void CreateProduct_Should_Return_Volume_In_CubicMeter(double x, double y, double z, DimensionUnit unit, double expectedVolume)
    {
        //Arrange
        var product = new Product
        {
            Dimensions = new Dimensions
            {
                X = x,
                Y = y,
                Z = z,
                Unit = unit
            }
        };

        //Act
        var volume = product.Dimensions.VolumeInCubicMeter;

        //Assert
        Assert.Equal(expectedVolume, volume);
    }
}