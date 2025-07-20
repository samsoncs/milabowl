using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Milabowl.Processing;
using Milabowl.Processing.DataImport;
using Milabowl.Snapshot;

var services = DependencyInjection.GetServices();
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
var snapshotPath = configuration["SnapshotPath"]!;
var descriptors = services.Where(d => d.ServiceType == typeof(IFplService)).ToList();
foreach (var descriptor in descriptors)
{
    services.Remove(descriptor);
}
services.AddHttpClient<IFplService, FplService>()
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new SnapshotDelegatingHandler(snapshotPath);
        handler.InnerHandler = new HttpClientHandler();
        return handler;
    });
var serviceProvider = services.BuildServiceProvider();

Console.WriteLine("Starting snapshot");
var importer = serviceProvider.GetRequiredService<FplImporter>();
await importer.ImportFplDataForRulesProcessing();
Console.WriteLine("Snapshot complete");
