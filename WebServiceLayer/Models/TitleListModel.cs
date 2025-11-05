namespace WebServiceLayer.Models
{
    public class TitleListModel
    {
        public string? URL { get; set; }
        public string? TitleType { get; set; }
        public required string PrimaryTitle { get; set; }
        public string? ReleaseDate { get; set; }
        public short? TotalSeasons { get; set; }
        public string? Poster { get; set; }
        public string? AllCastURL { get; set; }
    }
}
