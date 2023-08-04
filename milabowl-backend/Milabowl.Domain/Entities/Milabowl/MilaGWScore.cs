namespace Milabowl.Domain.Entities.Milabowl;

public class MilaGWScore
{ 
    public Guid MilaGWScoreId { get; set; }
    public string GW { get; set; }
    public string TeamName { get; set; }
    public decimal CapFail { get; set; }
    public decimal BenchFail { get; set; }
    public decimal CapKeep { get; set; }
    public decimal CapDef { get; set; }
    public decimal GWPosition { get; set; }
    public decimal GWPositionScore { get; set; }
    public decimal GW69 { get; set; }
    public decimal RedCard { get; set; }
    public decimal YellowCard { get; set; }
    public decimal MinusIsPlus { get; set; }
    public decimal IncreaseStreak { get; set; }
    public decimal EqualStreak { get; set; }
    public decimal GWScore { get; set; }
    public int GameWeek { get; set; }
    public string UserName { get; set; }
    public decimal MilaPoints { get; set; }
    public decimal HeadToHeadMeta { get; set; }
    //public decimal HeadToHeadStrongOpponentWin { get; set; }
    public decimal UniqueCap { get; set; }
    public decimal SixtyNineSub { get; set; }
    public decimal TrendyBitch { get; set; }
    public string? ActiveChip { get; set; }
    public decimal Mushroom { get; set; }
    public decimal BlueShell { get; set; }
    public decimal GreenShell { get; set; }
    
    public void CalculateMilaPoints()
    {
        MilaPoints = CapFail
                     + CapKeep
                     + CapDef
                     + BenchFail
                     + GWPositionScore
                     + GW69
                     + RedCard
                     + YellowCard
                     + MinusIsPlus
                     + IncreaseStreak
                     + EqualStreak
                     + HeadToHeadMeta
                     + UniqueCap
                     + SixtyNineSub
                     + TrendyBitch;
    }

    public void CalculateChipPoints()
    {
        MilaPoints += BlueShell;
        MilaPoints += GreenShell;
        
        if (ActiveChip == "3xc")
        {
            Mushroom = MilaPoints;
            MilaPoints += Mushroom;
        }
    }
}