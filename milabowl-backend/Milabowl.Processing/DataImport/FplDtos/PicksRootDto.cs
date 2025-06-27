using System.Text.Json.Serialization;

namespace Milabowl.Processing.DataImport.FplDtos;

public record AutoSubDto
{
    [JsonPropertyName("entry")]
    public required int Entry { get; set; }
    [JsonPropertyName("element_in")]
    public required int ElementIn { get; set; }
    [JsonPropertyName("element_out")]
    public required int ElementOut { get; set; }
    [JsonPropertyName("event")]
    public required int Event { get; set; }
}

public record PicksRootDto
{
    [JsonPropertyName("active_chip")]
    public required string ActiveChip { get; init; }
    [JsonPropertyName("automatic_subs")]
    public required List<AutoSubDto> AutoSubs { get; set; }
    [JsonPropertyName("entry_history")]
    public required EntryHistoryDto EntryHistory { get; set; }
    [JsonPropertyName("picks")]
    public required List<PickDto> Picks { get; init; }
}
