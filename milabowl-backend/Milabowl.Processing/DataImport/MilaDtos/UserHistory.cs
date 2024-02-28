namespace Milabowl.Processing.DataImport.MilaDtos;

public class UserHistory
{
    public Guid UserHistoryId { get; set; }
    public User User { get; set; } = default!;
    public Guid FkUserId { get; set; }
    public string SeasonName { get; set; } = default!;
    public int TotalPoints { get; set; }
    public int Rank { get; set; }
}
