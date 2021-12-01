using System.Collections.Generic;

namespace Milabowl.Business.DTOs.Import
{
    public class ElementHistoryRootDTO
    {
        //public List<ElementHistoryFixtureDTO> fixtures { get; set; }
        public List<ElementHistoryDTO> history { get; set; }
        public List<ElementHistoryPastDTO> history_past { get; set; }
        public int FantasyElementId { get; set; }
    }
}
