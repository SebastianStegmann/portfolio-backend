using DataServiceLayer.Models.Title;
using WebServiceLayer.Models.DTO;

namespace WebServiceLayer.Models
{
    public class TitleModel
    {
        public string? URL { get; set; }
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
        public string? AllCastURL { get; set; }

        public string? GenresURL { get; set; }
        public string? AkaURL { get; set; }
        public string? EpisodesURL { get; set; }
        public RatingDTO? OverallRating { get; set; }
        public string? Awards {  get; set; }

    }
}
