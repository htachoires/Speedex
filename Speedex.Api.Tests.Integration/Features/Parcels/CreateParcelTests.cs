using System.Net;
using System.Text;
using System.Text.Json;
using Speedex.Api.Features.Parcels.Requests;
using Speedex.Api.Features.Parcels.Responses;
using Speedex.Api.Tests.Integration.Features.Parcels.Request;
using Speedex.Domain.Orders.Repositories;
using Speedex.Domain.Products.Repositories;
using Speedex.Tests.Tools.TestDataBuilders.Domain.Orders;

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
    public async Task TODO_CreateParcel_Should_ReturnBadRequest_When_ProductIdIsNotFound()
    {
        // Arrange
        var httpClient = _factory.CreateClient();
        var order = AnOrder.Build();
        
        _factory.Services.GetRequiredService<IOrderRepository>().UpsertOrder(order);
        
        var request = new CreateParcelTestBodyRequest
        {
            OrderId = order.OrderId.Value,
            Products =
            [
                new CreateParcelTestBodyRequest.ParcelProductCreateParcelTestBodyRequest()
                {
                    ProductId = Guid.NewGuid().ToString(),
                    Quantity = 1
                }
            ]
        };

        var bodyContent = JsonSerializer.Serialize(request);

        // Act
        var response = await httpClient.PostAsync("/Parcels", new StringContent(bodyContent, Encoding.UTF8, "application/json"));

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("IsExistingProductValidator", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task TODO_CreateParcel_Should_ReturnBadRequest_When_OrderIdIsNotFound()
    {
        // Arrange
        var httpClient = _factory.CreateClient();
        var product = AProduct.Build();
        
        _factory.Services.GetRequiredService<IProductRepository>().UpsertProduct(product);
        var request = new CreateParcelTestBodyRequest
        {
            OrderId = Guid.NewGuid().ToString(),
            Products =
            [
                new CreateParcelTestBodyRequest.ParcelProductCreateParcelTestBodyRequest()
                {
                    ProductId = product.ProductId.Value,
                    Quantity = 1
                }
            ]
        };

        var bodyContent = JsonSerializer.Serialize(request);

        // Act
        var response = await httpClient.PostAsync("/Parcels", new StringContent(bodyContent, Encoding.UTF8, "application/json"));
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("IsExistingOrderValidator", await response.Content.ReadAsStringAsync());
    }
    
    [Fact]
    public async Task Should_ReturnTrue_When_ProductIsInsideOrder()
    {
        // Arrange : we make an order which contains an orderproduct
        var httpClient = _factory.CreateClient();

        var orderProduct = OrderProductBuilder.AnOrderProduct.WithProductId("notFoo").Build();
        var order = AnOrder
            .WithProduct(orderProduct) // Add custom product.
            .Build();
        
        _factory.Services.GetRequiredService<IOrderRepository>().UpsertOrder(order);
        //_factory.Services.GetRequiredService<IProductRepository>().UpsertProduct(orderProduct);
        
        // Act
        var retrievedOrder = await _factory.Services.GetRequiredService<IOrderRepository>()
            .GetOrderById(order.OrderId.Value);

        // Assert
        Assert.Contains(retrievedOrder!.Products, p => p.ProductId.Value == orderProduct.ProductId.Value);
        
    }
    
    [Fact]
    public async Task Should_ReturnFalse_When_ProductIsNotInsideOrder()
    {
        // Arrange : we make an order which contains an orderproduct
        var httpClient = _factory.CreateClient();

        var orderProduct = OrderProductBuilder.AnOrderProduct.WithProductId("notFoo").Build();
        var order = AnOrder // Add custom product.
            .Build();
        
        _factory.Services.GetRequiredService<IOrderRepository>().UpsertOrder(order);
        //_factory.Services.GetRequiredService<IProductRepository>().UpsertProduct(orderProduct);
        
        // Act
        var retrievedOrder = await _factory.Services.GetRequiredService<IOrderRepository>()
            .GetOrderById(order.OrderId.Value);

        // Assert
        Assert.DoesNotContain(retrievedOrder!.Products, p => p.ProductId.Value == orderProduct.ProductId.Value);
        
    }
}