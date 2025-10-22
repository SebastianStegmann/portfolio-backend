using DataServiceLayer.Models;

namespace DataServiceLayer;

public abstract class BaseDataService
{
    protected readonly ImdbContext _context;

    protected BaseDataService(ImdbContext context)
    {
        _context = context;
    }
}
