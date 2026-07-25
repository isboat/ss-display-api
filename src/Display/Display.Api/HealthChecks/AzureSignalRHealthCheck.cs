using Display.Models;
using Microsoft.Azure.SignalR.Management;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace Display.Api.HealthChecks;

public sealed class AzureSignalRHealthCheck : IHealthCheck
{
    private readonly SignalRSettings _settings;
    private readonly ILoggerFactory _loggerFactory;

    public AzureSignalRHealthCheck(
        IOptions<SignalRSettings> settings,
        ILoggerFactory loggerFactory)
    {
        _settings = settings.Value;
        _loggerFactory = loggerFactory;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var serviceManager = new ServiceManagerBuilder()
                .WithOptions(options =>
                {
                    options.ConnectionString = _settings.AzureSignalRConnectionString;
                    options.ServiceTransportType = ServiceTransportType.Transient;
                })
                .WithLoggerFactory(_loggerFactory)
                .BuildServiceManager();

            var isHealthy = await serviceManager.IsServiceHealthy(cancellationToken);

            return isHealthy
                ? HealthCheckResult.Healthy("Azure SignalR reported that the service is available.")
                : HealthCheckResult.Unhealthy("Azure SignalR reported that the service is unavailable.");
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Unhealthy(
                "The Azure SignalR health request failed.",
                exception);
        }
    }
}
