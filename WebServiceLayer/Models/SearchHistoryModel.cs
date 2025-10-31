namespace WebServiceLayer.Models
{
    public class SearchHistoryModel
    {
        public string? URL { get; set; }
        public int PersonId { get; set; }
        public string Search_string { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
