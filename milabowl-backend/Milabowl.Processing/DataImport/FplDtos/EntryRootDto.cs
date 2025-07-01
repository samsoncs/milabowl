using System.Text.Json.Serialization;

namespace Milabowl.Processing.DataImport.FplDtos;

public record EntryRootDto([property: JsonPropertyName("past")] IList<EntryResultDto> Past);

public record EntryResultDto(
    [property: JsonPropertyName("season_name")] string SeasonName,
    [property: JsonPropertyName("total_points")] int TotalPoints,
    [property: JsonPropertyName("rank")] int Rank
);
