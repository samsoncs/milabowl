using System.Text.Json.Serialization;

namespace Milabowl.Processing.DataImport.FplDtos;

public class BootstrapRootDTO
{
    public required List<EventDTO> Events { get; init; }
    public required List<TeamDTO> Teams { get; init; }

    [JsonPropertyName("total_players")]
    public required int TotalPlayers { get; init; }

    [JsonPropertyName("elements")]
    public required List<PlayerDTO> Players { get; init; }
}
