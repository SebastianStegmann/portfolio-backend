namespace DataServiceLayer.Models.Person;

public class IndividualRating
{
    public int PersonId { get; set; }
    public required string Tconst { get; set; }
    public int RatingValue { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
