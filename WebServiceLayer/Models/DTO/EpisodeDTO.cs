namespace WebServiceLayer.Models.DTO
{
    public class EpisodeDTO
    {
        public string? URL { get; set; }
        public string PrimaryTitle { get; set; } = string.Empty;
        public short? SeasonNumber { get; set; }
        public short? EpisodeNumber { get; set; }
        public string? ReleaseDate { get; set; }
    }
}
