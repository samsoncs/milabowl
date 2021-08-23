namespace Milabowl.Business.DTOs.Api
{
    public class MilaResultDTO
    {
        public MilaRulePointsDTO MilaPoints { get; set; }
        public string GW { get; set; }
        public decimal GWScore { get; set; }
        public string TeamName { get; set; }
        public string UserName { get; set; }
        public decimal GWPosition { get; set; }
        public int GameWeek { get; set; }
        public decimal? CumulativeMilaPoints { get; set; }
        public decimal CumulativeAverageMilaPoints { get; set; }
        public decimal TotalCumulativeAverageMilaPoints { get; set; }
        public int? MilaRank { get; set; }
        public int? MilaRankLastWeek { get; set; }

    }
}
