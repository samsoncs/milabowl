using System.Collections.Generic;

namespace Milabowl.Business.DTOs.Import
{
    public class StandingsDTO
    {
        public bool has_next { get; set; }
        public int page { get; set; }
        public List<ResultDTO> results { get; set; }
    }
}
