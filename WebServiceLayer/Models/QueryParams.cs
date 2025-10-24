namespace WebServiceLayer.Models;

public class QueryParams
{
    private const int MaxPageSize = 25;

    private int pageSize = 10;
    public int PageSize
    {
        get { return pageSize; }
        set { pageSize = value > MaxPageSize ? MaxPageSize : (value < 1 ? 10 : value); }
    }

    public int Page { get; set; } = 0;
}