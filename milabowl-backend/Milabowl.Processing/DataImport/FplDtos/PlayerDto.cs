using System.Text.Json.Serialization;

namespace Milabowl.Processing.DataImport.FplDtos;

public record PlayerDto
{
    [JsonPropertyName("code")]
    public required int Code { get; set; }

    [JsonPropertyName("element_type")]
    public required int ElementType { get; set; }

    [JsonPropertyName("event_points")]
    public required int EventPoints { get; set; }

    [JsonPropertyName("first_name")]
    public required string FirstName { get; set; }

    [JsonPropertyName("form")]
    public required string Form { get; set; }

    [JsonPropertyName("id")]
    public required int Id { get; set; }

    [JsonPropertyName("in_dreamteam")]
    public required bool InDreamteam { get; set; }

    [JsonPropertyName("news")]
    public required string News { get; set; }

    [JsonPropertyName("news_added")]
    public DateTime? NewsAdded { get; set; }

    [JsonPropertyName("now_cost")]
    public required int NowCost { get; set; }

    [JsonPropertyName("photo")]
    public required string Photo { get; set; }

    [JsonPropertyName("points_per_game")]
    public required string PointsPerGame { get; set; }

    [JsonPropertyName("second_name")]
    public required string SecondName { get; set; }

    [JsonPropertyName("selected_by_percent")]
    public required string SelectedByPercent { get; set; }

    [JsonPropertyName("special")]
    public required bool Special { get; set; }

    [JsonPropertyName("squad_number")]
    public int? SquadNumber { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("team")]
    public required int Team { get; set; }

    [JsonPropertyName("team_code")]
    public required int TeamCode { get; set; }

    [JsonPropertyName("transfers_in")]
    public required int TransfersIn { get; set; }

    [JsonPropertyName("transfers_in_event")]
    public required int TransfersInEvent { get; set; }

    [JsonPropertyName("transfers_out")]
    public required int TransfersOut { get; set; }

    [JsonPropertyName("transfers_out_event")]
    public required int TransfersOutEvent { get; set; }

    [JsonPropertyName("value_form")]
    public required string ValueForm { get; set; }

    [JsonPropertyName("value_season")]
    public required string ValueSeason { get; set; }

    [JsonPropertyName("web_name")]
    public required string WebName { get; set; }

    [JsonPropertyName("minutes")]
    public required int Minutes { get; set; }

    [JsonPropertyName("goals_scored")]
    public required int GoalsScored { get; set; }

    [JsonPropertyName("assists")]
    public required int Assists { get; set; }

    [JsonPropertyName("clean_sheets")]
    public required int CleanSheets { get; set; }

    [JsonPropertyName("goals_conceded")]
    public required int GoalsConceded { get; set; }

    [JsonPropertyName("own_goals")]
    public required int OwnGoals { get; set; }

    [JsonPropertyName("penalties_saved")]
    public required int PenaltiesSaved { get; set; }

    [JsonPropertyName("penalties_missed")]
    public required int PenaltiesMissed { get; set; }

    [JsonPropertyName("yellow_cards")]
    public required int YellowCards { get; set; }

    [JsonPropertyName("red_cards")]
    public required int RedCards { get; set; }

    [JsonPropertyName("saves")]
    public required int Saves { get; set; }

    [JsonPropertyName("bonus")]
    public required int Bonus { get; set; }

    [JsonPropertyName("bps")]
    public required int Bps { get; set; }

    [JsonPropertyName("influence")]
    public required string Influence { get; set; }

    [JsonPropertyName("creativity")]
    public required string Creativity { get; set; }

    [JsonPropertyName("threat")]
    public required string Threat { get; set; }

    [JsonPropertyName("ict_index")]
    public required string IctIndex { get; set; }

    [JsonPropertyName("corners_and_indirect_freekicks_order")]
    public int? CornersAndIndirectFreekicksOrder { get; set; }

    [JsonPropertyName("corners_and_indirect_freekicks_text")]
    public required string CornersAndIndirectFreekicksText { get; set; }

    [JsonPropertyName("direct_freekicks_order")]
    public int? DirectFreekicksOrder { get; set; }

    [JsonPropertyName("direct_freekicks_text")]
    public required string DirectFreekicksText { get; set; }

    [JsonPropertyName("penalties_order")]
    public int? PenaltiesOrder { get; set; }

    [JsonPropertyName("penalties_text")]
    public required string PenaltiesText { get; set; }
}
