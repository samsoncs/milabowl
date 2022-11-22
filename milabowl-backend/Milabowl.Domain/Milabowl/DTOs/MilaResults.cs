namespace Milabowl.Domain.Milabowl.DTOs;

public class MilaResults
{
    public IList<GameWeekResults> ResultsByWeek { get; set; }
    public IList<UserResults> ResultsByUser { get; set; }
    public IList<MilaResult> OverallScore { get; set; }
}

public class GameWeekResults
{
    public IList<MilaResult> Results { get; set; }
    public int GameWeek { get; set; }
}

public class UserResults
{
    public IList<MilaResult> Results { get; set; }
    public string TeamName { get; set; }
}