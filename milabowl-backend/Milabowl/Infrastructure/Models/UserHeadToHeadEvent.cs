using System;

namespace Milabowl.Infrastructure.Models
{
    public class UserHeadToHeadEvent
    {
        public Guid UserHeadToHeadEventID { get; set; }
        public int FantasyUserHeadToHeadEventID { get; set; }
        public User User { get; set; }
        public Guid FkUserId { get; set; }
        public int Points { get; set; }
        public int Win { get; set; }
        public int Draw { get; set; }
        public int Loss { get; set; }
        public int Total { get; set; }
        public Event Event { get; set; }
        public Guid FkEventId { get; set; }
        public bool IsKnockout { get; set; }
        public int LeagueID { get; set; }
        public bool IsBye { get; set; }
    }
}
