namespace DataServiceLayer.Models.Person;

public class Bookmark
{
    public int PersonId { get; set; }
    public required string Tconst { get; set; }
    public DateTime CreatedAt { get; set; }
}
