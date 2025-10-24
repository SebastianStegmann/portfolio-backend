namespace WebServiceLayer.Models;

public class QueryParams
{
    private const int MaxPageSize = 25;
    public int PageSize { get; set; } = 5;

    private int page = 0;

    public int Page
    {
        get { return page; }
        set { page = value > MaxPageSize ? MaxPageSize : value; }
    }
}