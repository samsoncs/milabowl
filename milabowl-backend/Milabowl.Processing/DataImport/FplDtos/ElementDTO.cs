namespace Milabowl.Processing.DataImport.FplDtos;

public class ElementDTO
{
    public int id { get; set; }
    public StatsDTO stats { get; set; }
    public List<ExplainDTO> explain { get; set; }
}
