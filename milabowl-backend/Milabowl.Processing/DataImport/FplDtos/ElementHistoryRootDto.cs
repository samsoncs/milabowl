namespace Milabowl.Processing.DataImport.FplDtos;

public class ElementHistoryRootDto
{
    //public List<ElementHistoryFixtureDTO> fixtures { get; set; }
    public required List<ElementHistoryDTO> history { get; init; }
    public required List<ElementHistoryPastDTO> history_past { get; init; }
    public int FantasyElementId { get; set; }
}
