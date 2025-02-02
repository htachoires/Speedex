using System.Text.Json;

namespace Speedex.Api.Tests.Integration.Features.Products;

public class CreateProductTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public CreateProductTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateProduct_Should_ReturnCreatedStatusCode_And_FindCreatedProduct()
    {
        //TODO(lvl-3) Implement the test using AAA pattern
        // You can use the Order, Parcel and Return tests as a reference
    }
}