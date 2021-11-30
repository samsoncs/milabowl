using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Milabowl.Infrastructure.Models
{
    public class Event
    {
        [Key]
        public Guid EventId { get; set; }
        public int FantasyEventId { get; set; }
        public DateTime Deadline { get; set; }
        public bool Finished { get; set; }
        public bool DataChecked { get; set; }
        public string Name { get; set; }
        public int GameWeek { get; set; }
        public int MostSelectedPlayerID { get; set; }
        public int MostTransferredInPlayerID { get; set; }
        public int MostCaptainedPlayerID { get; set; }
        public int MostViceCaptainedPlayerID { get; set; }

        public IList<PlayerEvent> PlayerEvents { get; set; }
        public IList<UserHeadToHeadEvent> PlayerHeadToHeadEvents { get; set; }
        public IList<Lineup> Lineups { get; set; }
    }
}
