using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Milabowl.Infrastructure.Models
{
    public class Team
    {
        [Key]
        public Guid TeamId { get; set; }
        public int FantasyTeamId { get; set; }
        public int FantasyTeamCode { get; set; }
        public string TeamName { get; set; }
        public string TeamShortName { get; set; }
        public virtual IList<Player> Players { get; set; }
    }
}
