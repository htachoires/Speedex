using System.Diagnostics;
using Speedex.Data;

namespace Speedex.Api.Bootstrap.HostedServices;

public class DataGeneratorHostedService(IDataGenerator dataGenerator, ILogger<DataGeneratorHostedService> logger) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting {MethodName}...", nameof(IDataGenerator.GenerateData));
        var stopwatch = Stopwatch.StartNew();

        dataGenerator.GenerateData();

        stopwatch.Stop();
        logger.LogInformation("{MethodName} completed in {ElapsedSeconds:F2} seconds", nameof(IDataGenerator.GenerateData), stopwatch.Elapsed.TotalSeconds);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}