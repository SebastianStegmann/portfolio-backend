namespace DataServiceLayer.Models;

public class SearchHistory
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public string Search_string { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
