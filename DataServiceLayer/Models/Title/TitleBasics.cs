namespace DataServiceLayer.Models.TitleBasics;

public class TitleBasics
{
    public string Tconst { get; set; } = string.Empty;
    public string? TitleType { get; set; }
    public string PrimaryTitle { get; set; } = string.Empty;
    public string OriginalTitle { get; set; } = string.Empty;
    public bool? IsAdult { get; set; }
    public string? ReleaseDate { get; set; }
    public string? EndYear { get; set; }
    public short? TotalSeasons { get; set; }
    public string? Plot { get; set; }
    public string? Poster { get; set; }
    public string? Country { get; set; }
    public int? RuntimeMinutes { get; set; }
}
