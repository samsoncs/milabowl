using System.Collections.Generic;

namespace Milabowl.Business.DTOs.Import
{
    public class PicksRootDTO
    {
        public object active_chip { get; set; }
        public List<object> automatic_subs { get; set; }
        public EntryHistoryDTO entry_history { get; set; }
        public List<PickDTO> picks { get; set; }
    }
}
