using System.Text.Json.Serialization;

namespace Milabowl.Processing.DataImport.FplDtos;

public record EntryRootDTO(
    [property: JsonPropertyName("past")] IList<EntryResultDTO> Past
);
    
    
public record EntryResultDTO(
    [property: JsonPropertyName("season_name")] string SeasonName,
    [property: JsonPropertyName("total_points")] int TotalPoints,
    [property: JsonPropertyName("rank")] int Rank
);
    