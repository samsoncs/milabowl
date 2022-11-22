namespace Milabowl.Domain.Import.FantasyDTOs;

public class NewEntriesDTO
{
    public bool has_next { get; set; }
    public int page { get; set; }
    public List<object> results { get; set; }
}