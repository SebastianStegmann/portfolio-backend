namespace WebServiceLayer.Models
{
    public class RatingsModel
    {
        public string Tconst { get; set; } = null!;
        public int RatingValue { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? TitleURL { get; set; }
    }
}
