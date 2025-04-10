using Prometheus;

namespace WorkerService1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        // Define Prometheus metrics
        private static readonly Counter _loopCounter = Metrics.CreateCounter("worker_loop_total", "Total number of worker loop executions");
        private static readonly Gauge _isRunningGauge = Metrics.CreateGauge("worker_running", "Indicates if the worker is running (1 = yes, 0 = no)");
        private static readonly Histogram _executionTime = Metrics.CreateHistogram("worker_loop_duration_seconds", "Duration of worker loop iteration in seconds");

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _isRunningGauge.Set(1); // Mark as running

            while (!stoppingToken.IsCancellationRequested)
            {
                using (_executionTime.NewTimer()) // Measure how long this loop iteration takes
                {
                    _loopCounter.Inc(); // Increment loop counter
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    await Task.Delay(1000, stoppingToken);
                }
            }

            _isRunningGauge.Set(0); // Mark as not running when cancelled
        }
    }
}
