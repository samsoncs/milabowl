using Microsoft.Extensions.DependencyInjection;
using Milabowl.Milalytics;
using Milabowl.Processing;
using Milabowl.Processing.DataImport;
using Milabowl.Processing.Utils;

var wasSuccessful = Migrator.Migrate();
if (!wasSuccessful)
{
    Console.WriteLine("Failed to migrate");
    return;
}

var serviceCollection = new ServiceCollection();
serviceCollection.AddMilabowlServices(SnapshotMode.None);
var serviceProvider = serviceCollection.BuildServiceProvider();
var importer = serviceProvider.GetRequiredService<FplImporter>();
await FplDataImporter.ImportToSql(importer);
