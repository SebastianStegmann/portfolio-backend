namespace WebServiceLayer.Models;

public class QueryParams
{
    private const int MaxPageSize = 25;

    private int pageSize = 10;
    public int PageSize
    {
        get { return pageSize; }
        set { pageSize = Math.Max(1, Math.Min(value, MaxPageSize)); }}

    public int Page { get; set; } = 0;
}
