using System.Text.Json.Serialization;

namespace Milabowl.Processing.DataImport.FplDtos;

public record ResultDto
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }
    [JsonPropertyName("event_total")]
    public required int EventTotal { get; set; }
    [JsonPropertyName("player_name")]
    public required string PlayerName { get; set; }
    [JsonPropertyName("rank")]
    public required int Rank { get; set; }
    [JsonPropertyName("last_rank")]
    public required int LastRank { get; set; }
    [JsonPropertyName("rank_sort")]
    public required int RankSort { get; set; }
    [JsonPropertyName("total")]
    public required int Total { get; set; }
    [JsonPropertyName("entry")]
    public required int Entry { get; set; }
    [JsonPropertyName("entry_name")]
    public required string EntryName { get; set; }
}
