namespace Milabowl.Domain.Import.FantasyDTOs;

public class ElementDTO
{
    public int id { get; set; }
    public StatsDTO stats { get; set; }
    public List<ExplainDTO> explain { get; set; }
}