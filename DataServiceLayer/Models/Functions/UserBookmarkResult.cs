namespace DataServiceLayer.Models.Functions;

public class UserBookmarkResult
{
    public string Type { get; set; } = null!;
    public string Id { get; set; } = null!;
    public string Title_Or_Name { get; set; } = null!;
    public DateTime Created_At { get; set; }
}
