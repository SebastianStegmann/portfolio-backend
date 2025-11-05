namespace DataServiceLayer.Models.Person;

public class SearchHistory
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public required string Search_string { get; set; }
    public DateTime CreatedAt { get; set; }
}
