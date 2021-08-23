using System.Collections.Generic;

namespace Milabowl.Business.DTOs.Import
{
    public class ExplainDTO
    {
        public int fixture { get; set; }
        public List<StatDTO> stats { get; set; }
    }
}
