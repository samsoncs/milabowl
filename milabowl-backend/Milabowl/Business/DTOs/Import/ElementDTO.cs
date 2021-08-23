using System.Collections.Generic;

namespace Milabowl.Business.DTOs.Import
{
    public class ElementDTO
    {
        public int id { get; set; }
        public StatsDTO stats { get; set; }
        public List<ExplainDTO> explain { get; set; }
    }
}
