using DataServiceLayer.Models;
namespace DataServiceLayer;

public class DataService
{
  private readonly ImdbContext _context;

  public DataService()
  {
    _context = new ImdbContext();
  }

  public List<TitleBasics> GetTitles()
  {
    return _context.TitleBasics.ToList();
  }

  public TitleBasics? GetTitle(string tconst)
  {
    return _context.TitleBasics.Find(tconst);
  }

  // person
  public List<Person> GetPerson()
  {
    return _context.Person.ToList();
  }

  public Person? GetPerson(int Id)
  {
    return _context.Person.Find(Id);
  }

  // search history
  public List<SearchHistory> GetSearchHistories()
  {
      return _context.SearchHistory.ToList();
  }

  public SearchHistory? GetSearchHistory(int id)
  {
      return _context.SearchHistory.Find(id);
  }
  public List<SearchHistory> GetSearchHistoriesByPersonId(int personId)
  {
    return _context.SearchHistory.Where(sh => sh.PersonId == personId).ToList();
  }

  // bookmarks
  public List<Bookmark> GetBookmarksByPersonId(int personId)
  {
    return _context.Bookmark.Where(b => b.PersonId == personId).ToList();
  }

  // rating
  public List<Rating> GetRatingsByPersonId(int personId)
  {
    return _context.Rating.Where(r => r.PersonId == personId).ToList();
  }
}
