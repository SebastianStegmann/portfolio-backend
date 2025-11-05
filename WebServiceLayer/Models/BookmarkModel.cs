namespace WebServiceLayer.Models
{
    public class BookmarkModel
    {
        public required string Tconst { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? TitleURL { get; set; }
    }
}
