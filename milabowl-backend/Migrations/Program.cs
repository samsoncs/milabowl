using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Milabowl.Domain.Import;
using Milabowl.Domain.Milabowl;
using Milabowl.Domain.Processing;
using Milabowl.Infrastructure.Contexts;
using Milabowl.Infrastructure.Repositories;
using IMilaResultsService = Milabowl.Domain.Milabowl.IMilaResultsService;

const string CONNECTION_STRING = "Persist Security Info=False;UID=SA;Pwd=!5omeSup3rF4ncyPwd!;Database=fantasy;Server=localhost,1431; Connection Timeout=30;TrustServerCertificate=True";

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddScoped<IMilaResultsService, MilaResultsService>();
        services.AddScoped<IDataImportBusiness, DataImportBusiness>();
        services.AddScoped<IMilaPointsProcessorService, MilaPointsProcessorService>();
        services.AddScoped<IMilaRuleBusiness, MilaRuleBusiness>();
        services.AddScoped<IDataImportService, DataImportService>();
        services.AddScoped<IMilaResultsBusiness, MilaResultsBusiness>();
        services.AddScoped<IDataImportProvider, DataImportProvider>();
        services.AddScoped<IFantasyMapper, FantasyMapper>();
        services.AddScoped<IImportRepository, ImportRepository>();
        services.AddScoped<IProcessingRepository, ProcessingRepository>();
        services.AddScoped<IMilaRepository, MilaRepository>();
        services.AddDbContext<FantasyContext>(optionsBuilder =>
        {
            optionsBuilder.UseSqlServer(CONNECTION_STRING, options => options.EnableRetryOnFailure());
        });
        services.AddHttpClient();
    }).Build();

await Migrate();
await Import(host.Services, args[0]);

static async Task Migrate()
{
    Console.WriteLine("Running migrations...");
    var dbContextBuilder = new DbContextOptionsBuilder<FantasyContext>(new DbContextOptions<FantasyContext>());
    dbContextBuilder.UseSqlServer(CONNECTION_STRING);
    await using var db = new FantasyContext(dbContextBuilder.Options);
    await db.Database.MigrateAsync();
    Console.WriteLine("Finished running migrations");
}

static async Task Import(IServiceProvider services, string filePath)
{
    Console.WriteLine("Importing data");
    var dataImportService = services.GetRequiredService<IDataImportService>()!;
    var milaPointsProcessorService = services.GetRequiredService<IMilaPointsProcessorService>()!;
    var milaResultsService = services.GetRequiredService<IMilaResultsService>()!;

    // await dataImportService.ImportData();
    // await milaPointsProcessorService.UpdateMilaPoints();
    var fplResults = await milaResultsService.GetFplResults();
    var fplJson = JsonSerializer.Serialize(fplResults, new JsonSerializerOptions{ PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
    await File.WriteAllTextAsync($"C:\\Users\\SamsonSvendsen\\Garbage\\fpl_state.json", fplJson);
    
    var milaResults = await milaResultsService.GetMilaResults();

    var json = JsonSerializer.Serialize(milaResults, new JsonSerializerOptions{ PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
    
    await File.WriteAllTextAsync($"{filePath}/game_state.json", json);
    Console.WriteLine("Finished importing data");
}