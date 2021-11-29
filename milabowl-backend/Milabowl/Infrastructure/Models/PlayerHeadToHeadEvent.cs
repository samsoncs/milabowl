using System;

namespace Milabowl.Infrastructure.Models
{
    public class PlayerHeadToHeadEvent
    {
        public Guid PlayerHeadToHeadEventID { get; set; }
        public int FantasyPlayerHeadToHeadEventID { get; set; }
        public Player Entry1_Player { get; set; }
        public int Entry1Points { get; set; }
        public int Entry1Win { get; set; }
        public int Entry1Draw { get; set; }
        public int Entry1Loss { get; set; }
        public int Entry1Total { get; set; }

        public Player Entry2_Player { get; set; }
        public int Entry2Points { get; set; }
        public int Entry2Win { get; set; }
        public int Entry2Draw { get; set; }
        public int Entry2Loss { get; set; }
        public int Entry2Total { get; set; }

        public Event Event { get; set; }
        public bool IsKnockout { get; set; }
        public int LeagueID { get; set; }
        public bool IsBye { get; set; }
    }
}
