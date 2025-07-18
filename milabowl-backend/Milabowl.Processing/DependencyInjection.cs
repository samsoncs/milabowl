using Microsoft.Extensions.DependencyInjection;
using Milabowl.Processing.DataImport;
using Milabowl.Processing.Processing;
using Scrutor;

namespace Milabowl.Processing;

public static class DependencyInjection
{
    public static IServiceProvider GetServiceProvider()
    {
        var serviceCollection = GetServices();
        return serviceCollection.BuildServiceProvider();
    }

    public static IServiceCollection GetServices()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddTransient<Processor>();
        serviceCollection.AddTransient<HistorySummarizer>();
        serviceCollection.AddTransient<IRulesProcessor, RulesProcessor>();
        serviceCollection.AddTransient<FplImporter>();
        serviceCollection.AddHttpClient<IFplService, FplService>();
        serviceCollection.Scan(s =>
            s.FromAssemblyOf<IMilaRule>()
                .AddClasses()
                .AsSelfWithInterfaces()
                .WithTransientLifetime()
        );
        serviceCollection.AddSingleton<IBombState, BombState>();
        return serviceCollection;
    }
}
