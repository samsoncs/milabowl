namespace Milabowl.Domain.Import.FantasyDTOs;

public class HeadToHeadEventRootDTO
{
    public bool has_next { get; set; }
    public int page { get; set; }
    public List<HeadToHeadResultDTO> results { get; set; }
}