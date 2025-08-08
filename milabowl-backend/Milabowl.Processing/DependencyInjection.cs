using Microsoft.Extensions.DependencyInjection;
using Milabowl.Processing.DataImport;
using Milabowl.Processing.Processing;
using Milabowl.Processing.Processing.BombState;
using Milabowl.Processing.Utils;
using Milabowl.Processing.Utils.Snapshot;

namespace Milabowl.Processing;

public static class DependencyInjection
{
    public static void AddMilabowlServices(
        this IServiceCollection serviceCollection,
        SnapshotMode snapshotMode
    )
    {
        serviceCollection.AddTransient<Processor>();
        serviceCollection.AddTransient<HistorySummarizer>();
        serviceCollection.AddTransient<IRulesProcessor, RulesProcessor>();
        serviceCollection.AddTransient<FplImporter>();
        serviceCollection.AddTransient<IFilePathResolver, FilePathResolver>();
        serviceCollection.AddTransient<IFileSystem, FileSystem>();
        serviceCollection.AddTransient<ISnapshotPathResolver, SnapshotPathResolver>();
        serviceCollection.AddFplService(snapshotMode);
        serviceCollection.AddSingleton<IBombState, BombState>();

        serviceCollection.Scan(s =>
            s.FromAssemblyOf<IMilaRule>()
                .AddClasses(classes => classes.AssignableTo<IMilaRule>())
                .AsSelfWithInterfaces()
                .WithTransientLifetime()
        );
    }

    private static void AddFplService(this IServiceCollection services, SnapshotMode snapshotMode)
    {
        switch (snapshotMode)
        {
            case SnapshotMode.Read:
                Console.WriteLine("Using snapshot instead of live API");
                services.AddTransient<SnapshotReadHandler>();
                services
                    .AddHttpClient<IFplService, FplService>()
                    .AddHttpMessageHandler<SnapshotReadHandler>();
                break;
            case SnapshotMode.Write:
                Console.WriteLine("Writing FPL snapshots when making requests");
                services.AddTransient<SnapshotHandler>();
                services
                    .AddHttpClient<IFplService, FplService>()
                    .AddHttpMessageHandler<SnapshotHandler>();
                break;
            case SnapshotMode.None:
            default:
                Console.WriteLine("Using live FPL API");
                services.AddHttpClient<IFplService, FplService>();
                break;
        }
    }
}
