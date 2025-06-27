using Microsoft.Extensions.DependencyInjection;
using Milabowl.Processing;
using Milabowl.Processing.Processing;

var serviceProvider = DependencyInjection.GetServiceProvider();
var processor = serviceProvider.GetRequiredService<Processor>();
await processor.ProcessMilaPoints(args[0]);
