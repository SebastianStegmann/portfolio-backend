namespace DataServiceLayer.Models.Title;

public class TitleEpisode
{
    public required string Tconst { get; set; }
    public string? ParentTconst { get; set; }
    public required string PrimaryTitle { get; set; }
    public required string OriginalTitle { get; set; }
    public bool? IsAdult { get; set; }
    public string? ReleaseDate { get; set; }
    public int? RuntimeMinutes { get; set; }
    public string? Poster { get; set; }
    public string? Plot { get; set; }
    public short? SeasonNumber { get; set; }
    public short? EpisodeNumber { get; set; }
}
