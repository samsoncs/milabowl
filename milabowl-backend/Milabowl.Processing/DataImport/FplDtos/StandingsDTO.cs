namespace Milabowl.Processing.DataImport.FplDtos;

public class StandingsDTO
{
    public bool has_next { get; set; }
    public int page { get; set; }
    public List<ResultDTO> results { get; set; }
}