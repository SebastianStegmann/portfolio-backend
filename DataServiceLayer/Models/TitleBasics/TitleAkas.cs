namespace DataServiceLayer.Models.TitleBasics;

public class TitleAka
{
    public string Tconst { get; set; } = string.Empty;
    public short Ordering { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Region { get; set; }
    public string? Language { get; set; }
    public string? Types { get; set; }
    public string? Attributes { get; set; }
    public bool? IsOriginalTitle { get; set; }
}
