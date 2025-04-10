using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Prometheus;

namespace WorkerService1
{

    public class MetricsServerHostedService : IHostedService
    {
        private IHost? _metricsHost;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _metricsHost = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(options =>
                    {
                        options.ListenAnyIP(9090); // Expose port 9090
                    });

                    webBuilder.Configure(app =>
                    {
                        app.UseRouting();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapMetrics(); // /metrics endpoint
                        });
                    });
                })
                .Build();


            Console.WriteLine("Metrics server started - available at http://localhost:9090/metrics");
            return _metricsHost.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _metricsHost?.StopAsync(cancellationToken) ?? Task.CompletedTask;
        }
    }
}

