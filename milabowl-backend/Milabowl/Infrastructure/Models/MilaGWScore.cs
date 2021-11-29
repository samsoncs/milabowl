using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Milabowl.Infrastructure.Models
{
    public class MilaGWScore
    {
        [Key]
        public Guid MilaGWScoreId { get; set; }
        public string GW { get; set; }
        public string TeamName{ get; set; }
        public decimal Hit { get; set; }
        public decimal CapFail { get; set; }
        public decimal BenchFail { get; set; }
        public decimal CapKeep { get; set; }
        public decimal CapDef { get; set; }
        public decimal GWPosition { get; set; }
        public decimal GWPositionScore { get; set; }
        public decimal GW69 { get; set; }
        public decimal RedCard { get; set; }
        public decimal YellowCard { get; set;}
        public decimal MinusIsPlus { get; set; }
        public decimal IncreaseStreak { get; set; }
        public decimal EqualStreak { get; set; }
        public decimal GWScore {get; set;}
        public int GameWeek { get; set; }
        public string UserName { get; set; }
        public decimal MilaPoints { get; set; }

        public decimal TrendyBitch { get; set; }
   }
}
