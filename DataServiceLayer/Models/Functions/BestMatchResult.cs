namespace DataServiceLayer.Models.Functions;

public class BestMatchResult
{
    public required string Tconst { get; set; }
    public required string Primarytitle { get; set; }
    public int Match_Count { get; set; }
}
