using System.Text;
using System.Text.Json;
using Speedex.Api.Features.Returns.Requests;
using Speedex.Api.Features.Returns.Responses;

namespace Speedex.Api.Tests.Integration;

public class CreateReturnTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public CreateReturnTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateReturn_ShouldReturnCreatedAndRessourceLocationInHeaders()
    {
        // Arrange
        var httpClient = _factory.CreateClient();

        var request = new CreateReturnBodyRequest
        {
            OrderId = Guid.NewGuid().ToString(),
            ParcelId = Guid.NewGuid().ToString(),
            Products =
            [
                new CreateReturnBodyRequest.CreateReturnBodyRequestReturnProduct()
                {
                    ProductId = Guid.NewGuid().ToString(),
                    Quantity = 1
                }
            ]
        };

        // Act
        var response = await httpClient.PostAsync("/Returns", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

        // Assert
        response.EnsureSuccessStatusCode();

        var location = response.Headers.Location.ToString();

        var getResponse = await httpClient.GetAsync(location);

        var content = await getResponse.Content.ReadAsStringAsync();
        var getReturnsResponse = JsonSerializer.Deserialize<GetReturnsResponse>(content, _jsonSerializerOptions);

        Assert.Single(getReturnsResponse!.Items);
    }
}