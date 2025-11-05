namespace WebServiceLayer.Models
{
    public class SearchHistoryModel
    {
        public string? URL { get; set; }
        public int PersonId { get; set; }
        public required string Search_string { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
