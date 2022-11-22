namespace Milabowl.Domain.Import.FantasyDTOs;

public class ElementHistoryDTO
{
    public int element { get; set; }
    public int fixture { get; set; }
    public int opponent_team { get; set; }
    public int total_points { get; set; }
    public bool was_home { get; set; }
    public DateTime kickoff_time { get; set; }
    public int? team_h_score { get; set; }
    public int? team_a_score { get; set; }
    public int round { get; set; }
    public int minutes { get; set; }
    public int goals_scored { get; set; }
    public int assists { get; set; }
    public int clean_sheets { get; set; }
    public int goals_conceded { get; set; }
    public int own_goals { get; set; }
    public int penalties_saved { get; set; }
    public int penalties_missed { get; set; }
    public int yellow_cards { get; set; }
    public int red_cards { get; set; }
    public int saves { get; set; }
    public int bonus { get; set; }
    public int bps { get; set; }
    public string influence { get; set; }
    public string creativity { get; set; }
    public string threat { get; set; }
    public string ict_index { get; set; }
    public int value { get; set; }
    public int transfers_balance { get; set; }
    public int selected { get; set; }
    public int transfers_in { get; set; }
    public int transfers_out { get; set; }
}