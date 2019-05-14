using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode.Runner
{
    public static class Program
    {
        public static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public static void ConfigureAppConfiguration(IConfigurationBuilder builder) =>
            builder.AddCommandLine(Environment.GetCommandLineArgs());

        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddHostedService<RunnerService>();
        }

        static Task Main(string[] args)
        {
            return new HostBuilder()
                   //.ConfigureHostConfiguration(ConfigureHostConfiguration)
                   .ConfigureAppConfiguration(ConfigureAppConfiguration)
                   .ConfigureServices(ConfigureServices)
                   //.ConfigureContainer<Container>(ConfigureContainer)
                   .UseConsoleLifetime()
                   .RunConsoleAsync(cancellationTokenSource.Token);
        }
    }

    public class RunnerService : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Program.cancellationTokenSource.Cancel();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
