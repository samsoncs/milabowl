namespace Milabowl.Domain.Import.FantasyDTOs;

public class PicksRootDTO
{
    public object active_chip { get; set; }
    public List<object> automatic_subs { get; set; }
    public EntryHistoryDTO entry_history { get; set; }
    public List<PickDTO> picks { get; set; }
}