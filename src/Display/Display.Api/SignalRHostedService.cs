using Display.Models;
using Microsoft.Azure.SignalR.Management;
using Microsoft.Extensions.Options;

namespace Display.Api
{


    public interface IHubContextStore
    {
        public ServiceHubContext MessageHubContext { get; }
    }

    public class SignalRHostedService : IHostedService, IHubContextStore
    {
        private readonly SignalRSettings _settings;
        private readonly ILoggerFactory _loggerFactory;

        public ServiceHubContext MessageHubContext { get; private set; } = null!;

        public SignalRHostedService(IOptions<SignalRSettings> settings, ILoggerFactory loggerFactory)
        {
            _settings = settings.Value;
            _loggerFactory = loggerFactory;
        }

        async Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            using var serviceManager = new ServiceManagerBuilder()
                .WithOptions(o => {
                    o.ConnectionString = _settings.AzureSignalRConnectionString;
                    o.ServiceTransportType = ServiceTransportType.Transient;
                })
                .WithLoggerFactory(_loggerFactory)
                .BuildServiceManager();

            MessageHubContext = await serviceManager.CreateHubContextAsync(SignalRConstants.HubName, cancellationToken);
        }

        Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            return Task.WhenAll(Dispose(MessageHubContext));
        }

        private static Task Dispose(ServiceHubContext? hubContext)
        {
            if (hubContext == null)
            {
                return Task.CompletedTask;
            }
            return hubContext.DisposeAsync();
        }
    }
}
