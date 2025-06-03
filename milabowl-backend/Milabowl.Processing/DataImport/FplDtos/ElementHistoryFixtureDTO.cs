namespace Milabowl.Processing.DataImport.FplDtos;

public class ElementHistoryFixtureDTO
{
    public int id { get; set; }
    public int code { get; set; }
    public int team_h { get; set; }
    public int team_h_score { get; set; }
    public int team_a { get; set; }
    public int team_a_score { get; set; }
    public int @event { get; set; }
    public bool finished { get; set; }
    public int minutes { get; set; }
    public bool provisional_start_time { get; set; }
    public DateTime kickoff_time { get; set; }
    public string event_name { get; set; }
    public bool is_home { get; set; }
    public int difficulty { get; set; }
}
