using DataServiceLayer.Models;
using DataServiceLayer.Models.NameBasics;
namespace DataServiceLayer;

public class TitleDataService : BaseDataService
{
  public TitleDataService(ImdbContext context) : base(context) { }

    //Title

  public List<TitleBasics> GetTitles()
  {
    return _context.TitleBasics.ToList();
  }

  public TitleBasics? GetTitle(string tconst)
  {
    return _context.TitleBasics.Find(tconst);
  }

    public List<TitleBasics> GetAllGenres()
    {
        return _context.TitleBasics.ToList();
    }
}

