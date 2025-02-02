using Microsoft.AspNetCore.Mvc.Testing;

namespace Speedex.Api.Tests.Integration;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            //TODO: register custom services if needed
        });

        builder.UseEnvironment("Development");
    }
}