namespace Milabowl.Processing.DataImport.FplDtos;

public class PicksRootDTO
{
    public string active_chip { get; set; }
    public List<object> automatic_subs { get; set; }
    public EntryHistoryDTO entry_history { get; set; }
    public List<PickDTO> picks { get; set; }
}