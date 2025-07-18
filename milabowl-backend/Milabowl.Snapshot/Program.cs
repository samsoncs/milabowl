using Microsoft.Extensions.DependencyInjection;
using Milabowl.Processing;
using Milabowl.Processing.DataImport;
using Milabowl.Snapshot;

var services = DependencyInjection.GetServices();
var repoRoot = Path.GetDirectoryName(AppContext.BaseDirectory)!;
var snapshotPath = Path.Combine(repoRoot, "snapshots", "24-25");
services.AddHttpClient<IFplService, FplService>()
    .ConfigurePrimaryHttpMessageHandler(() => new SnapshotDelegatingHandler(snapshotPath));
var serviceProvider = services.BuildServiceProvider();

Console.WriteLine("Starting snapshot");
var importer = serviceProvider.GetRequiredService<FplImporter>();
Console.WriteLine("Snapshot complete");
