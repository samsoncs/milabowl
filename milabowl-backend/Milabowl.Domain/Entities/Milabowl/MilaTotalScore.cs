namespace Milabowl.Domain.Entities.Milabowl;

public class MilaTotalScore
{
    public Guid MilaTotalScoreId { get; set; }
    public string UserName { get; set; }
    public string TeamName { get; set; }
    public decimal H2H { get; set; }
    public decimal MaxGWScore { get; set; }
    public decimal MinGWScore { get; set; }
    public decimal TeamValue { get; set; }
    public decimal NoHands { get; set; }

}