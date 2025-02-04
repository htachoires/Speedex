using System.Text.Json;
using Speedex.Api.Features.Parcels.Requests;

namespace Speedex.Api.Tests.Integration.Features.Parcels;

public class CreateParcelTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public CreateParcelTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task TODO_CreateParcel_Should_ReturnCreatedStatusCode_And_FindCreatedParcel()
    {
        // Arrange
        var httpClient = _factory.CreateClient();

        var request = new CreateParcelBodyRequest
        {
            OrderId = Guid.NewGuid().ToString(),
            Products =
            [
                new CreateParcelBodyRequest.ParcelProductCreateParcelBodyRequest()
                {
                    ProductId = Guid.NewGuid().ToString(),
                    Quantity = 1
                }
            ]
        };

        var bodyContent = JsonSerializer.Serialize(request);

        // Act
        //TODO(lvl-2) Implement call parcel creation endpoint
        //var response = null;
        //
        // Assert
        //TODO(lvl-2) Uncomment the following lines when the response is implemented
        //Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        //
        // var location = response.Headers.Location.ToString();
        //
        // var getResponse = await httpClient.GetAsync(location);
        //
        // var content = await getResponse.Content.ReadAsStringAsync();
        // var getParcelsResponse = JsonSerializer.Deserialize<GetParcelsResponse>(content, _jsonSerializerOptions);
        //
        // Assert.Single(getParcelsResponse!.Items);
    }
}