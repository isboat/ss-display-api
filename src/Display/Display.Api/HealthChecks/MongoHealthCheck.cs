using Display.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Display.Api.HealthChecks;

public sealed class MongoHealthCheck : IHealthCheck
{
    private readonly MongoClient _client;

    public MongoHealthCheck(IOptions<MongoSettings> settings)
    {
        _client = new MongoClient(settings.Value.ConnectionString);
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _client
                .GetDatabase("admin")
                .RunCommandAsync<BsonDocument>(
                    new BsonDocument("ping", 1),
                    cancellationToken: cancellationToken);

            return HealthCheckResult.Healthy("MongoDB accepted a ping command.");
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Unhealthy(
                "MongoDB did not respond to a ping command.",
                exception);
        }
    }
}
