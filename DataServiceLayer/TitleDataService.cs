using DataServiceLayer.Models;
using DataServiceLayer.Models.NameBasics;
using DataServiceLayer.Models.TitleBasics;
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

  public List<Genre> GetAllGenres()
  {
    return _context.Genre.ToList();
  }

  // awards 
  public List<Award> GetAwardsByTconst(string tconst)
  {
    return _context.Awards.Where(a => a.Tconst == tconst).ToList();
  } 
  
  // overall rating
  public List<OverallRating> GetOverallRatings(string tconst)
  {
    return _context.OverallRatings.Where(a => a.Tconst == tconst).ToList();
  }
}

