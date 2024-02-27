using System.Text.Json.Serialization;

namespace Milabowl.Processing.DataImport.FplDtos;

public class TeamDTO
{
    public int Code { get; set; }
    public int Draw { get; set; }
    public object Form { get; set; }
    public int Id { get; set; }
    public int Loss { get; set; }
    public string Name { get; set; }
    public int Played { get; set; }
    public int Points { get; set; }
    public int Position { get; set; }
    [JsonPropertyName("short_name")]
    public string ShortName { get; set; }
    public int Strength { get; set; }
    [JsonPropertyName("team_division")]
    public object TeamDivision { get; set; }
    public bool Unavailable { get; set; }
    public int Win { get; set; }
    [JsonPropertyName("strength_overall_home")]
    public int StrengthOverallHome { get; set; }
    [JsonPropertyName("strength_overall_away")]
    public int StrengthOverallAway { get; set; }
    [JsonPropertyName("strength_attack_home")]
    public int StrengthAttackHome { get; set; }
    [JsonPropertyName("strength_attack_away")]
    public int StrengthAttackAway { get; set; }
    [JsonPropertyName("strength_defence_home")]
    public int StrengthDefenceHome { get; set; }
    [JsonPropertyName("strength_defence_away")]
    public int StrengthDefenceAway { get; set; }
    [JsonPropertyName("pulse_id")]
    public int PulseId { get; set; }
}