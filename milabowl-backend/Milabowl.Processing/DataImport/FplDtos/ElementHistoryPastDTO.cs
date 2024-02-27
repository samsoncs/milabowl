namespace Milabowl.Processing.DataImport.FplDtos;

public class ElementHistoryPastDTO
{
    public string season_name { get; set; }
    public int element_code { get; set; }
    public int start_cost { get; set; }
    public int end_cost { get; set; }
    public int total_points { get; set; }
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
}