namespace DataServiceLayer.Models;

public class Bookmark
{
    public int PersonId { get; set; }
    public string Tconst { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}