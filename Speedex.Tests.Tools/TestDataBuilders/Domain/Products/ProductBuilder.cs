using Speedex.Domain.Products;

namespace Speedex.Tests.Tools.TestDataBuilders.Domain.Products;

public class ProductBuilder
{
    private ProductId _productId = new ProductId(Guid.NewGuid().ToString());
    private string _name = "DefaultProductName";
    private string _description = "DefaultDescription";
    private string _category = "DefaultCategory";
    private string _secondLevelCategory = "DefaultSecondLevelCategory";
    private string _thirdLevelCategory = "DefaultThirdLevelCategory";
    private Price _price = new Price { Amount = 19.99m, Currency = Currency.USD };
    private Dimensions _dimensions = new Dimensions { X = 0.1, Y = 0.2, Z = 0.3, Unit = DimensionUnit.Cm };
    private Weight _weight = new Weight { Value = 0.3, Unit = WeightUnit.Kg };
    private DateTime _creationDate = DateTime.Now;
    private DateTime _updateDate = DateTime.Now;

    public static ProductBuilder AProduct => new();

    public Product Build()
    {
        return new Product
        {
            ProductId = _productId,
            Name = _name,
            Description = _description,
            Category = _category,
            SecondLevelCategory = _secondLevelCategory,
            ThirdLevelCategory = _thirdLevelCategory,
            Price = _price,
            Dimensions = _dimensions,
            Weight = _weight,
            CreationDate = _creationDate,
            UpdateDate = _updateDate,
        };
    }

    public ProductBuilder Id(ProductId id)
    {
        _productId = id;
        return this;
    }

    public ProductBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProductBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public ProductBuilder WithCategory(string category)
    {
        _category = category;
        return this;
    }

    public ProductBuilder WithSecondLevelCategory(string secondLevelCategory)
    {
        _secondLevelCategory = secondLevelCategory;
        return this;
    }

    public ProductBuilder WithThirdLevelCategory(string thirdLevelCategory)
    {
        _thirdLevelCategory = thirdLevelCategory;
        return this;
    }

    public ProductBuilder WithPrice(decimal amount, Currency currency)
    {
        _price = new Price { Amount = amount, Currency = currency };
        return this;
    }

    public ProductBuilder WithDimensions(double x, double y, double z, DimensionUnit unit)
    {
        _dimensions = new Dimensions { X = x, Y = y, Z = z, Unit = unit };
        return this;
    }

    public ProductBuilder WithWeight(double value, WeightUnit unit)
    {
        _weight = new Weight { Value = value, Unit = unit };
        return this;
    }

    public ProductBuilder CreatedOn(DateTime creationDate)
    {
        _creationDate = creationDate;
        return this;
    }

    public ProductBuilder UpdatedOn(DateTime updateDate)
    {
        _updateDate = updateDate;
        return this;
    }
}