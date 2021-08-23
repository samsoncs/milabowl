using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Milabowl.Infrastructure.Models
{
    public class Player
    {
        [Key]
        public Guid PlayerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int NowCost { get; set; }
        public int FantasyPlayerId { get; set; }
        public virtual Team Team { get; set; }
        public Guid FkTeamId { get; set; }
        public virtual IList<PlayerEvent> PlayerEvents { get; set; }
        public int Code { get; set; }
        public int ElementType { get; set; }
        public int EventPoints { get; set; }
        public string Form { get; set; }
        public bool InDreamteam { get; set; }
        public string News { get; set; }
        public DateTime? NewsAdded { get; set; }
        public string Photo { get; set; }
        public string PointsPerGame { get; set; }
        public string SelectedByPercent { get; set; }
        public bool Special { get; set; }
        public string Status { get; set; }
        public int TotalPoints { get; set; }
        public int TransfersIn { get; set; }
        public int TransfersInEvent { get; set; }
        public int TransfersOut { get; set; }
        public int TransfersOutEvent { get; set; }
        public string ValueForm { get; set; }
        public string ValueSeason { get; set; }
        public string WebName { get; set; }
        public int Minutes { get; set; }
        public int GoalsScored { get; set; }
        public int Assists { get; set; }
        public int CleanSheets { get; set; }
        public int GoalsConceded { get; set; }
        public int OwnGoals { get; set; }
        public int PenaltiesSaved { get; set; }
        public int PenaltiesMissed { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
        public int Saves { get; set; }
        public int Bonus { get; set; }
        public int Bps { get; set; }
        public string Influence { get; set; }
        public string Creativity { get; set; }
        public string Threat { get; set; }
    }
}
