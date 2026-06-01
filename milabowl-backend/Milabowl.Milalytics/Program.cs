using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Milabowl.Milalytics;
using Milabowl.Processing;
using Milabowl.Processing.DataImport;
using Milabowl.Processing.Processing.BombState;
using Milabowl.Processing.Utils;

var wasSuccessful = Migrator.Migrate();
if (!wasSuccessful)
{
    Console.WriteLine("Failed to migrate");
    return;
}

var host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
        logging.SetMinimumLevel(LogLevel.Warning);
    })
    .ConfigureServices(
        (context, services) =>
        {
            var fplApiOptionsSection = context.Configuration.GetSection("FplApi");
            var fplApiOptions = fplApiOptionsSection.Get<FplApiOptions>()!;
            services.AddMilabowlServices(fplApiOptions.SnapshotMode);
            services.Configure<FplApiOptions>(fplApiOptionsSection);
            services.Configure<BombSettings>(context.Configuration.GetSection("BombSettings"));
        }
    )
    .Build();

var importer = host.Services.GetRequiredService<FplImporter>();
await FplDataImporter.ImportToSql(importer);
