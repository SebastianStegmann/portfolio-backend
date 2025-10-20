using DataServiceLayer.Models;
namespace DataServiceLayer;

public class DataService
{
  private readonly ImdbContext _context;

  public DataService(ImdbContext context)
  {
    _context = context;
  }

  public List<TitleBasics> GetTitles()
  {
    return _context.TitleBasics.ToList();
  }

  public TitleBasics? GetTitle(string tconst)
  {
    return _context.TitleBasics.Find(tconst);
  }

}
