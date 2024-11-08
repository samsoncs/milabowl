﻿using Microsoft.Extensions.DependencyInjection;
using Milabowl.Processing.Processing;

namespace Milabowl.Processing;

public static class DependencyInjection
{
    public static IServiceProvider GetServiceProvider()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddTransient<Processor>();
        serviceCollection.AddTransient<IRulesProcessor, RulesProcessor>();

        serviceCollection.Scan(s => 
            s.FromAssemblyOf<IMilaRule>()
                .AddClasses()
                .AsImplementedInterfaces()
        );

        return serviceCollection.BuildServiceProvider();
    }
}