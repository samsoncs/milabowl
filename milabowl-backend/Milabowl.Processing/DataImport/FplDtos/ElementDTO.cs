namespace Milabowl.Processing.DataImport.FplDtos;

public class ElementDTO
{
    public int id { get; set; }
    public required StatsDTO stats { get; init; }
    public required List<ExplainDTO> explain { get; init; }
}
