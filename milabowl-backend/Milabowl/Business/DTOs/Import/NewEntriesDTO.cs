using System.Collections.Generic;

namespace Milabowl.Business.DTOs.Import
{
    public class NewEntriesDTO
    {
        public bool has_next { get; set; }
        public int page { get; set; }
        public List<object> results { get; set; }
    }
}
