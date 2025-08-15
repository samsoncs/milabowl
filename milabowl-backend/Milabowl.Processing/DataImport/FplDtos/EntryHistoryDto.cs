using System.Text.Json.Serialization;

namespace Milabowl.Processing.DataImport.FplDtos;

public record EntryHistoryDto
{
    [JsonPropertyName("points_on_bench")]
    public required int PointsOnBench { get; init; }

    [JsonPropertyName("event_transfers_cost")]
    public required int EventTransfersCost { get; init; }

    [JsonPropertyName("event_transfers")]
    public required int EventTransfers { get; init; }

    [JsonPropertyName("bank")]
    public required int Bank { get; init; }

    [JsonPropertyName("value")]
    public required int Value { get; init; }

    [JsonPropertyName("percentile_rank")]
    public int? PercentileRank { get; init; }

    [JsonPropertyName("overall_rank")]
    public int? OverallRank { get; init; }

    [JsonPropertyName("rank_sort")]
    public int? RankSort { get; init; }

    [JsonPropertyName("rank")]
    public int? Rank { get; init; }

    [JsonPropertyName("total_points")]
    public required int TotalPoints { get; init; }

    [JsonPropertyName("points")]
    public required int Points { get; init; }

    [JsonPropertyName("event")]
    public required int Event { get; init; }
}
