namespace DataServiceLayer.Models.Person;

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime? Birthday { get; set; }
    public string? Location { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public DateTime? LastLogin { get; set; }
    public DateTime CreatedAt { get; set; }
}
