using System.Text.Json.Serialization;

namespace Milabowl.Domain.Import.FantasyDTOs;

public class BootstrapRootDTO
{
    public List<EventDTO> Events { get; set; }
    public List<TeamDTO> Teams { get; set; }
    [JsonPropertyName("total_players")]
    public int TotalPlayers { get; set; }
    [JsonPropertyName("elements")]
    public List<PlayerDTO> Players { get; set; }
}