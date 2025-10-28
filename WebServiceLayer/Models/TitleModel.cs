using DataServiceLayer.Models.Title;

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

        public ICollection<TitleGenre> Genre { get; set; } = new List<TitleGenre>();
        public ICollection<TitleAka> Aka { get; set; } = new List<TitleAka>();
        public ICollection<TitleEpisode> Episodes { get; set; } = new List<TitleEpisode>();

    }
}
