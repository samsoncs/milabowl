using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Milabowl.Processing;
using Milabowl.Processing.Processing;
using Milabowl.Processing.Utils;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
        logging.SetMinimumLevel(LogLevel.Warning);
        logging.AddFilter("System.Net.Http.HttpClient", LogLevel.Warning);
        logging.AddFilter("System.Net.Http.HttpClient.IFplService", LogLevel.Warning);
    })
    .ConfigureServices(
        (context, services) =>
        {
            var fplApiOptionsSection = context.Configuration.GetSection("FplApi");
            var fplApiOptions = fplApiOptionsSection.Get<FplApiOptions>()!;
            services.AddMilabowlServices(fplApiOptions.SnapshotMode);
            services.Configure<FplApiOptions>(fplApiOptionsSection);
        }
    )
    .Build();

var processor = host.Services.GetRequiredService<Processor>();
await processor.ProcessMilaPoints(args[0]);
