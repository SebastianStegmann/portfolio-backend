namespace DataServiceLayer.Models.Person;

public class Person
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime? Birthday { get; set; }
    public string? Location { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public DateTime? LastLogin { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<SearchHistory> Search { get; set; } = new List<SearchHistory>();
    public ICollection<IndividualRating> IndividualRating { get; set; } = new List<IndividualRating>();
    public ICollection<Bookmark> Bookmark { get; set; } = new List<Bookmark>();


}
