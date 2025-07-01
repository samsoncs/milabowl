using System.Text.Json.Serialization;

namespace Milabowl.Processing.DataImport.FplDtos
{
    public record LeagueRootDto
    {
        [JsonPropertyName("league")]
        public required LeagueDto League { get; set; }

        [JsonPropertyName("new_entries")]
        public required NewEntriesDto NewEntries { get; set; }

        [JsonPropertyName("standings")]
        public required StandingsDto Standings { get; set; }
    }
}
