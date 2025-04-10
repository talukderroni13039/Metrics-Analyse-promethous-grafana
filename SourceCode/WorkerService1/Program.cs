namespace WorkerService1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHostedService<Worker>();
            builder.Services.AddHostedService<MetricsServerHostedService>();
            var host = builder.Build();
            host.Run();


        }
    }
    // New class to host the metrics endpoint
    
}