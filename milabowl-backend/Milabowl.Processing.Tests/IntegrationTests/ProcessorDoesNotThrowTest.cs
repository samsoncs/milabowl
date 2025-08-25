using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Milabowl.Processing.Processing;
using Milabowl.Processing.Processing.BombState;
using Milabowl.Processing.Processing.BombState.Models;
using Milabowl.Processing.Utils;
using NSubstitute;
using Shouldly;

namespace Milabowl.Processing.Tests.IntegrationTests;

public class ProcessorDoesNotThrowTest
{
    private readonly IServiceCollection _serviceCollection;
    private readonly MyFileSystem _myFileSystem;

    private class MyFileSystem : IFileSystem
    {
        private readonly IDictionary<string, string?> _fileContents =
            new Dictionary<string, string?>();

        public Task WriteAllTextAsync(string path, string? contents)
        {
            _fileContents.TryAdd(path, contents);
            return Task.CompletedTask;
        }

        public T ReadFile<T>(string path)
        {
            if (!_fileContents.TryGetValue(path, out var contents) || contents is null)
            {
                throw new FileNotFoundException($"File not found: {path}");
            }

            return JsonSerializer.Deserialize<T>(
                contents,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Converters = { new JsonStringEnumConverter() },
                    PropertyNameCaseInsensitive = true,
                }
            )!;
        }
    }

    public ProcessorDoesNotThrowTest()
    {
        _serviceCollection = new ServiceCollection();
        _serviceCollection.AddSingleton<IOptions<FplApiOptions>>(_ =>
            Options.Create(
                new FplApiOptions
                {
                    SnapshotPath = "./Snapshots/24-25",
                    SnapshotMode = SnapshotMode.Read,
                    MainLeagueId = 1650213,
                    HeadToHeadLeagueId = 1649633,
                }
            )
        );
        _serviceCollection.AddSingleton<IOptions<BombSettings>>(_ =>
            Options.Create(new BombSettings { InitialBombHolder = 2216421, RandomSeed = 69 })
        );
        _serviceCollection.AddMilabowlServices(SnapshotMode.Read);
        _myFileSystem = new MyFileSystem();
        _serviceCollection.Replace(
            new ServiceDescriptor(
                typeof(IFileSystem),
                _ => _myFileSystem,
                ServiceLifetime.Transient
            )
        );
        var myPathResolver = Substitute.For<IFilePathResolver>();
        myPathResolver.ResolveGameStateFilePath().Returns("");
        _serviceCollection.Replace(
            new ServiceDescriptor(
                typeof(IFilePathResolver),
                _ => myPathResolver,
                ServiceLifetime.Transient
            )
        );
    }

    [Fact(Timeout = 30_000)]
    public async Task Should_process_without_throwing_exceptions()
    {
        // Arrange
        await using var serviceProvider = _serviceCollection.BuildServiceProvider();
        var processor = serviceProvider.GetRequiredService<Processor>();

        // Act
        await processor.ProcessMilaPoints();

        // Make some super basic assertions to ensure the game state was processed
        var gameState = _myFileSystem.ReadFile<MilaResults>("/game_state.json");
        gameState.Rules.Count.ShouldBeGreaterThan(10);
        gameState.OverallScore.Count.ShouldBeGreaterThanOrEqualTo(9);
        gameState.ResultsByUser.Count.ShouldBeGreaterThanOrEqualTo(9);

        var user = gameState.ResultsByUser[0];
        user.Results.Count.ShouldBeGreaterThan(1);
        user.Results[0].Rules.Count.ShouldBeGreaterThan(10);

        var bombHistoryDisplayValues = _myFileSystem.ReadFile<BombHistoryDisplayValues>(
            "/bomb_state.json"
        );
        var bombHistoryRows = bombHistoryDisplayValues.BombHistoryByGameWeek;
        bombHistoryRows.Count.ShouldBeGreaterThan(1);
        bombHistoryRows[0][0].Description.ShouldNotBeNull();
        bombHistoryRows[0][0].Emoji.ShouldNotBeNull();
    }
}
