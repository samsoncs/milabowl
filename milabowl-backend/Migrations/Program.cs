using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Milabowl;
using Milabowl.Business.Import;
using Milabowl.Infrastructure.Contexts;

namespace Migrations
{
    class Program
    {
        static void Main(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("./dockerappsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = configurationBuilder.Build();
            Startup.Configuration = configuration;

            if (args[0] == "Migrate")
            {
                Migrate();
            }
            else if (args[0] == "Import")
            {
                Migrate();
                Import().GetAwaiter().GetResult();
            }
        }

        static void Migrate()
        {
            Console.WriteLine("Running migrations...");
            var dbContextBuilder = new DbContextOptionsBuilder<FantasyContext>(new DbContextOptions<FantasyContext>());
            dbContextBuilder.UseSqlServer(Startup.Configuration.GetConnectionString("Milabowl"));
            using var db = new FantasyContext(dbContextBuilder.Options);
            db.Database.Migrate();
            Console.WriteLine("Finished running migrations");
        }

        static async Task Import()
        {
            Console.WriteLine("Importing data");
            var builder = new WebHostBuilder()
                .UseConfiguration(Startup.Configuration)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            var dataImportService = (DataImportService)builder.Services.GetService(typeof(IDataImportService));
            var milaPointsProcessorService = (MilaPointsProcessorService)builder.Services.GetService(typeof(IMilaPointsProcessorService));

            await dataImportService.ImportData();
            await milaPointsProcessorService.UpdateMilaPoints();
            Console.WriteLine("Finished importing data");
        }
    }
}
