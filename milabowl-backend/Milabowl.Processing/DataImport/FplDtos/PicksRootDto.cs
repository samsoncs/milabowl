using System.Text.Json.Serialization;

namespace Milabowl.Processing.DataImport.FplDtos;

public record PicksRootDto
{
    [JsonPropertyName("active_chip")]
    public required string ActiveChip { get; set; }
    [JsonPropertyName("automatic_subs")]
    public required List<object> AutomaticSubs { get; set; }
    [JsonPropertyName("entry_history")]
    public required EntryHistoryDto EntryHistory { get; set; }
    [JsonPropertyName("picks")]
    public required List<PickDto> Picks { get; set; }
}
