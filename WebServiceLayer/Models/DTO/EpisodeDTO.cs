namespace WebServiceLayer.Models.DTO
{
    public class EpisodeDTO
    {
        public string? URL { get; set; }
        public required string PrimaryTitle { get; set; }
        public short? SeasonNumber { get; set; }
        public short? EpisodeNumber { get; set; }
        public string? ReleaseDate { get; set; }
    }
}
