using Speedex.Domain.Commons;
using Speedex.Domain.Orders.UseCases.CreateOrder;
using Speedex.Domain.Orders.UseCases.GetOrders;
using Speedex.Domain.Parcels.UseCases.CreateParcel;
using Speedex.Domain.Parcels.UseCases.GetParcels;
using Speedex.Domain.Products.UseCases.CreateProduct;
using Speedex.Domain.Products.UseCases.GetProducts;
using Speedex.Domain.Returns.UseCases.CreateReturn;
using Speedex.Domain.Returns.UseCases.GetReturns;

namespace Speedex.Api.Bootstrap;

public static class DomainBootstrap
{
    public static IServiceCollection RegisterDomainServices(this IServiceCollection services)
    {
        services
            .RegisterCommandHandlers()
            .RegisterQueryHandlers();

        return services;
    }

    private static IServiceCollection RegisterCommandHandlers(this IServiceCollection services)
    {
        services
            .AddScoped<ICommandHandler<CreateOrderCommand, CreateOrderResult>, CreateOrderCommandHandler>()
            .AddScoped<ICommandHandler<CreateParcelCommand, CreateParcelResult>, CreateParcelCommandHandler>()
            .AddScoped<ICommandHandler<CreateReturnCommand, CreateReturnResult>, CreateReturnCommandHandler>()
            .AddScoped<ICommandHandler<CreateProductCommand, CreateProductResult>, CreateProductCommandHandler>();

        return services;
    }

    private static IServiceCollection RegisterQueryHandlers(this IServiceCollection services)
    {
        services
            .AddScoped<IQueryHandler<GetOrdersQuery, GetOrdersQueryResult>, GetOrdersQueryHandler>()
            .AddScoped<IQueryHandler<GetParcelsQuery, GetParcelsQueryResult>, GetParcelsQueryHandler>()
            .AddScoped<IQueryHandler<GetReturnsQuery, GetReturnsQueryResult>, GetReturnsQueryHandler>()
            .AddScoped<IQueryHandler<GetProductsQuery, GetProductsQueryResult>, GetProductsQueryHandler>();

        return services;
    }
}