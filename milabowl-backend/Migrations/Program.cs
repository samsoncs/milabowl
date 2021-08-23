using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Milabowl;
using Milabowl.Business.Api;
using Milabowl.Business.Import;
using Milabowl.Business.Mappers;
using Milabowl.Infrastructure.Contexts;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
            dbContextBuilder.UseSqlServer("");
            using var db = new FantasyContext(dbContextBuilder.Options);
            db.Database.Migrate();
            Console.WriteLine("Finished running migrations");
        }

        static async Task Import()
        {
            Console.WriteLine("Importing data");
            //var builder = new WebHostBuilder()
            //    .UseConfiguration(Startup.Configuration)
            //    .UseContentRoot(Directory.GetCurrentDirectory())
            //    .UseStartup<Startup>()
            //    .Build();

            //var dataImportService = (DataImportService)builder.Services.GetService(typeof(IDataImportService));
            //var milaPointsProcessorService = (MilaPointsProcessorService)builder.Services.GetService(typeof(IMilaPointsProcessorService));

            var serviceProvider = GetServiceProvider();
            var dataImportService = serviceProvider.GetService<IDataImportService>();
            var milaPointsProcessorService = serviceProvider.GetService<IMilaPointsProcessorService>();
            var milaResultsService = serviceProvider.GetService<IMilaResultsService>();

            await dataImportService.ImportData();
            await milaPointsProcessorService.UpdateMilaPoints();
            var milaResults = await milaResultsService.GetMilaResults();
            var json = JsonConvert.SerializeObject(milaResults, new JsonSerializerSettings{ ContractResolver = new DefaultContractResolver{ NamingStrategy = new CamelCaseNamingStrategy() }  });
            await File.WriteAllTextAsync("C:\\Programming\\Other\\milabowl\\game_state.json", json);
            Console.WriteLine("Finished importing data");
        }

        private static IServiceProvider GetServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddScoped<IMilaResultsService, MilaResultsService>();
            services.AddScoped<IDataImportBusiness, DataImportBusiness>();
            services.AddScoped<IMilaPointsProcessorService, MilaPointsProcessorService>();
            services.AddScoped<IMilaRuleBusiness, MilaRuleBusiness>();
            services.AddScoped<IDataImportService, DataImportService>();
            services.AddScoped<IMilaResultsBusiness, MilaResultsBusiness>();
            services.AddScoped<IDataImportProvider, DataImportProvider>();
            services.AddScoped<IFantasyMapper, FantasyMapper>();
            services.AddDbContext<FantasyContext>(optionsBuilder =>
            {
                var connectionString = "";
                optionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
            });
            services.AddHttpClient();

            return services.BuildServiceProvider(); ;
        }
    }
}
