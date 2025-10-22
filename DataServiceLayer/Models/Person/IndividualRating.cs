namespace DataServiceLayer.Models;

public class IndividualRating
{
    public int PersonId { get; set; }
    public string Tconst { get; set; } = null!;
    public int RatingValue { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}