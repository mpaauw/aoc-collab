using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AdventOfCode;
using System.Linq;
using System.IO;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Runner
{
    public static class Program
    {
        public static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public static void ConfigureAppConfiguration(IConfigurationBuilder builder) =>
            builder.AddCommandLine(Environment.GetCommandLineArgs());

        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.Configure<RunnerContext>(o => context.Configuration.Bind(o));
            services.AddHostedService<RunnerService>();
        }

        public static void ConfigureLogging(HostBuilderContext context, ILoggingBuilder logging)
        {
            logging.AddConsole();
        }

        static Task Main(string[] args)
        {
            return new HostBuilder()
                   //.ConfigureHostConfiguration(ConfigureHostConfiguration)
                   .ConfigureAppConfiguration(ConfigureAppConfiguration)
                   .ConfigureServices(ConfigureServices)
                   .ConfigureLogging(ConfigureLogging)
                   .UseConsoleLifetime()
                   .RunConsoleAsync(cancellationTokenSource.Token);
        }
    }

    public class RunnerContext
    {
        public IEnumerable<int> DaysToRun { get; set; }

        public bool RunParallel { get; set; }
    }

    public class RunnerService : IHostedService
    {
        private readonly RunnerContext context;
        private readonly ILogger<RunnerService> logger;
        private Task runningWork;

        public RunnerService(IOptionsSnapshot<RunnerContext> context, ILogger<RunnerService> logger)
        {
            this.context = context.Value;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            this.runningWork = Task.Factory.StartNew(Run);
            await this.runningWork;
            Program.cancellationTokenSource.Cancel();
        }

        public Task StopAsync(CancellationToken cancellationToken) => this.runningWork;

        public async Task Run()
        {
            IEnumerable<Type> dayTypes;

            if (this.context.DaysToRun == null)
            {
                dayTypes = typeof(IDay).Assembly.ExportedTypes.Where(o => typeof(IDay).IsAssignableFrom(o) && o.IsClass && !o.IsAbstract);
            }
            else
            {
                dayTypes = this.context.DaysToRun.Select(id => typeof(IDay).Assembly.GetType("Day" + id));
            }


            if (this.context.RunParallel)
            {
                await Task.WhenAll(dayTypes.Select(RunDay).ToList());
            }
            else
            {
                foreach (var dayType in dayTypes)
                {
                    await RunDay(dayType);
                }
            }
        }

        public async Task RunDay(Type dayType)
        {
            using (this.logger.BeginScope(new { Day = dayType.FullName }))
            {
                var day = (IDay)Activator.CreateInstance(dayType);
                string inputPath = $@"Input\{dayType.Name}.txt";

                if (!File.Exists(inputPath))
                {
                    this.logger.LogError("Count not find input for {0}", dayType.FullName);
                    return;
                }

                var input = await File.ReadAllTextAsync(inputPath);
                try
                {
                    this.logger.LogTrace("Starting {0} Part A", dayType.FullName);
                    var partAResult = day.RunPartA(input);
                    this.logger.LogInformation("{0} Part A Result: {1}", dayType.FullName, partAResult);
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Error running {0} Part A", dayType.FullName);
                }

                try
                {
                    this.logger.LogTrace("Starting {0} Part B", dayType.FullName);
                    var partBResult = day.RunPartB(input);
                    this.logger.LogInformation("{0} Part B Result: {1}", dayType.FullName, partBResult);
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Error running {0} Part B", dayType.FullName);
                }
            }
        }
    }
}
