using System.Text.Json.Serialization;

namespace Milabowl.Processing.DataImport.FplDtos;

public record ElementHistoryRootDto
{
    //public List<ElementHistoryFixtureDto> Fixtures { get; set; }
    [JsonPropertyName("history")]
    public required List<ElementHistoryDto> History { get; init; }

    [JsonPropertyName("history_past")]
    public required List<ElementHistoryPastDto> HistoryPast { get; init; }

    [JsonPropertyName("id")]
    public int FantasyElementId { get; set; }
}
