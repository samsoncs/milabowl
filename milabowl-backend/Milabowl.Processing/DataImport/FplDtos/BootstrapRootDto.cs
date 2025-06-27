using System.Text.Json.Serialization;

namespace Milabowl.Processing.DataImport.FplDtos;

public record BootstrapRootDto
{
    [JsonPropertyName("events")]
    public required List<EventDto> Events { get; init; }
    [JsonPropertyName("teams")]
    public required List<TeamDto> Teams { get; init; }

    [JsonPropertyName("total_players")]
    public required int TotalPlayers { get; init; }

    [JsonPropertyName("elements")]
    public required List<PlayerDto> Players { get; init; }
}
