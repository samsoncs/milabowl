namespace Milabowl.Processing.DataImport.FplDtos;

public class ElementHistoryDTO
{
    public required int element { get; init; }
    public required int fixture { get; init; }
    public required int opponent_team { get; init; }
    public required int total_points { get; init; }
    public required bool was_home { get; init; }
    public required DateTime kickoff_time { get; init; }
    public required int? team_h_score { get; init; }
    public required int? team_a_score { get; init; }
    public required int round { get; init; }
    public required int minutes { get; init; }
    public required int goals_scored { get; init; }
    public required int assists { get; init; }
    public required int clean_sheets { get; init; }
    public required int goals_conceded { get; init; }
    public required int own_goals { get; init; }
    public required int penalties_saved { get; init; }
    public required int penalties_missed { get; init; }
    public required int yellow_cards { get; init; }
    public required int red_cards { get; init; }
    public required int saves { get; init; }
    public required int bonus { get; init; }
    public required int bps { get; init; }
    public required string influence { get; init; }
    public required string creativity { get; init; }
    public required string threat { get; init; }
    public required string ict_index { get; init; }
    public required int value { get; init; }
    public required int transfers_balance { get; init; }
    public required int selected { get; init; }
    public required int transfers_in { get; init; }
    public required int transfers_out { get; init; }
}
