namespace DataServiceLayer.Models;

public class TitleEpisode
{
    public string Tconst { get; set; } = string.Empty;
    public string? ParentTconst { get; set; }
    public string PrimaryTitle { get; set; } = string.Empty;
    public string OriginalTitle { get; set; } = string.Empty;
    public bool? IsAdult { get; set; }
    public string? ReleaseDate { get; set; }
    public int? RuntimeMinutes { get; set; }
    public string? Poster { get; set; }
    public string? Plot { get; set; }
    public short? SeasonNumber { get; set; }
    public short? EpisodeNumber { get; set; }
}
