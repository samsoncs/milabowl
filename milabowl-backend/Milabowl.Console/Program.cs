using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Milabowl.Business.Api;
using Milabowl.Business.Import;
using Milabowl.Business.Mappers;
using Milabowl.Infrastructure.Contexts;
using Newtonsoft.Json;

namespace Milabowl.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            var serviceProvider = GetServiceProvider();
            var dataImportService = serviceProvider.GetService<IDataImportService>();
            var milaPointsProcessorService = serviceProvider.GetService<IMilaPointsProcessorService>();
            var milaResultsService = serviceProvider.GetService<IMilaResultsService>();

            await dataImportService.ImportData();
            await milaPointsProcessorService.UpdateMilaPoints();
            var milaResults = await milaResultsService.GetMilaResults();
            var json = JsonConvert.SerializeObject(milaResults);
            await File.WriteAllTextAsync("C:\\Programming\\Other\\milabowl\\game_state.json", json);
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
