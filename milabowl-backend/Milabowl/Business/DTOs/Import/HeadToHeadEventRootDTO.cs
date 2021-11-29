using System.Collections.Generic;

namespace Milabowl.Business.DTOs.Import
{
    public class HeadToHeadEventRootDTO
    {
        public bool has_next { get; set; }
        public int page { get; set; }
        public List<HeadToHeadResultDTO> results { get; set; }
    }
}
