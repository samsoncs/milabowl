namespace Milabowl.Processing.DataImport.FplDtos;

public class ElementHistoryFixtureDTO
{
    public required int id { get; init; }
    public required int code { get; init; }
    public required int team_h { get; init; }
    public required int team_h_score { get; init; }
    public required int team_a { get; init; }
    public required int team_a_score { get; init; }
    public required int @event { get; init; }
    public required bool finished { get; init; }
    public required int minutes { get; init; }
    public required bool provisional_start_time { get; init; }
    public required DateTime kickoff_time { get; init; }
    public required string event_name { get; init; }
    public required bool is_home { get; init; }
    public required int difficulty { get; init; }
}
