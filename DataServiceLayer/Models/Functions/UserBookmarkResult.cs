namespace DataServiceLayer.Models.Functions;

public class UserBookmarkResult
{
    public required string Type { get; set; }
    public required string Id { get; set; }
    public required string Title_Or_Name { get; set; }
    public DateTime Created_At { get; set; }
}
