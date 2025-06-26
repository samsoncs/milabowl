namespace Milabowl.Processing.DataImport.FplDtos;

public class ElementHistoryPastDTO
{
    public required string season_name { get; init; }
    public required int element_code { get; init; }
    public required int start_cost { get; init; }
    public required int end_cost { get; init; }
    public required int total_points { get; init; }
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
}
