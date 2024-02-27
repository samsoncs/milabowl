using System.Text.Json.Serialization;

namespace Milabowl.Processing.DataImport.FplDtos;

public class PlayerDTO
{
    public int Code { get; set; }
    [JsonPropertyName("element_type")]
    public int ElementType { get; set; }
    [JsonPropertyName("event_points")]
    public int EventPoints { get; set; }
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }
    public string Form { get; set; }
    public int Id { get; set; }
    [JsonPropertyName("in_dreamteam")]
    public bool InDreamteam { get; set; }
    public string News { get; set; }
    [JsonPropertyName("news_added")]
    public DateTime? NewsAdded { get; set; }
    [JsonPropertyName("now_cost")]
    public int NowCost { get; set; }
    public string Photo { get; set; }
    [JsonPropertyName("points_per_game")]
    public string PointsPerGame { get; set; }
    [JsonPropertyName("second_name")]
    public string SecondName { get; set; }
    [JsonPropertyName("selected_by_percent")]
    public string SelectedByPercent { get; set; }
    public bool Special { get; set; }
    [JsonPropertyName("squad_number")]
    public object SquadNumber { get; set; }
    public string Status { get; set; }
    public int Team { get; set; }
    [JsonPropertyName("team_code")]
    public int team_code { get; set; }
    [JsonPropertyName("total_points")]
    public int TotalPoints { get; set; }
    [JsonPropertyName("transfers_in")]
    public int TransfersIn { get; set; }
    [JsonPropertyName("transfers_in_event")]
    public int TransfersInEvent { get; set; }
    [JsonPropertyName("transfers_out")]
    public int TransfersOut { get; set; }
    [JsonPropertyName("transfers_out_event")]
    public int TransfersOutEvent { get; set; }
    [JsonPropertyName("value_form")]
    public string ValueForm { get; set; }
    [JsonPropertyName("value_season")]
    public string ValueSeason { get; set; }
    [JsonPropertyName("web_name")]
    public string WebName { get; set; }
    public int Minutes { get; set; }
    [JsonPropertyName("cost_change_event")]
    public int goals_scored { get; set; }
    public int Assists { get; set; }
    [JsonPropertyName("clean_sheets")]
    public int CleanSheets { get; set; }
    [JsonPropertyName("goals_conceded")]
    public int GoalsConceded { get; set; }
    [JsonPropertyName("own_goals")]
    public int OwnGoals { get; set; }
    [JsonPropertyName("penalties_saved")]
    public int PenaltiesSaved { get; set; }
    [JsonPropertyName("penalties_missed")]
    public int PenaltiesMissed { get; set; }
    [JsonPropertyName("yellow_cards")]
    public int YellowCards { get; set; }
    [JsonPropertyName("red_cards")]
    public int RedCards { get; set; }
    public int Saves { get; set; }
    public int Bonus { get; set; }
    public int Bps { get; set; }
    public string Influence { get; set; }
    public string Creativity { get; set; }
    public string Threat { get; set; }
}