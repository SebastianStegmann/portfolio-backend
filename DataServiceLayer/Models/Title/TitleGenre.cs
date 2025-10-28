namespace DataServiceLayer.Models.Title;

public class TitleGenre
{
    public string Tconst { get; set; } = string.Empty;
    public short GenreId { get; set; }

    // Navigation to Genre
    public Genre? Genre { get; set; }
}
