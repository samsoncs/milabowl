using Microsoft.Extensions.DependencyInjection;
using Milabowl.Milalytics;
using Milabowl.Processing;
using Milabowl.Processing.DataImport;

var wasSuccessful = Migrator.Migrate();
if (!wasSuccessful)
{
    Console.WriteLine("Failed to migrate");
    return;
}

var serviceProvider = DependencyInjection.GetServiceProvider();
var importer = serviceProvider.GetRequiredService<FplImporter>();
await FplDataImporter.ImportToSql(importer);

