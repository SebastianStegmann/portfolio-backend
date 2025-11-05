namespace DataServiceLayer.Models.Title;

public class TitleGenre
{
    public required string Tconst { get; set; }
    public short GenreId { get; set; }

    // Navigation to Genre
    public Genre? Genre { get; set; }
}
