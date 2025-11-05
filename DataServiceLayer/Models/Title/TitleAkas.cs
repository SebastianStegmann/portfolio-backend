namespace DataServiceLayer.Models.Title;

public class TitleAka
{
    public required string Tconst { get; set; }
    public short Ordering { get; set; }
    public required string Title { get; set; }
    public string? Region { get; set; }
    public string? Language { get; set; }
    public string? Types { get; set; }
    public string? Attributes { get; set; }
    public bool? IsOriginalTitle { get; set; }
}
