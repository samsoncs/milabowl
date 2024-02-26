using Microsoft.Extensions.DependencyInjection;
using Milabowl.Processing;
using Milabowl.Processing.Processing;

Console.WriteLine("Hello, World!");

var serviceProvider = DependencyInjection.GetServiceProvider();
var processor = serviceProvider.GetRequiredService<Processor>();

processor.ProcessMilaPoints();





