using Speedex.Domain.Orders.Repositories;
using Speedex.Domain.Parcels.Repositories;
using Speedex.Domain.Products.Repositories;
using Speedex.Domain.Returns.Repositories;
using Speedex.Infrastructure;

namespace Speedex.Api.Bootstrap;

public static class InfrastructureBootstrap
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services)
    {
        services
            .RegisterRepositories();

        return services;
    }

    private static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services
            .AddSingleton<IOrderRepository, InMemoryOrderRepository>()
            .AddSingleton<IParcelRepository, InMemoryParcelRepository>()
            .AddSingleton<IReturnRepository, InMemoryReturnRepository>()
            .AddSingleton<IProductRepository, InMemoryProductRepository>();

        return services;
    }
}