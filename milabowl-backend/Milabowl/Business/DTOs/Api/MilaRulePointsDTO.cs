namespace Milabowl.Business.DTOs.Api
{
    public class MilaRulePointsDTO
    {
        public decimal CapFail { get; set; }
        public decimal BenchFail { get; set; }
        public decimal CapKeep { get; set; }
        public decimal CapDef { get; set; }
        public decimal GW69 { get; set; }
        public decimal RedCard { get; set; }
        public decimal YellowCard { get; set; }
        public decimal MinusIsPlus { get; set; }
        public decimal IncreaseStreak { get; set; }
        public decimal EqualStreak { get; set; }
        public decimal Hit { get; set; }
        public decimal GWPositionScore { get; set; }
        public decimal? Total { get; set; }
    }
}
