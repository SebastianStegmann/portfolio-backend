using DataServiceLayer.Models;
using DataServiceLayer.Models.NameBasics;
namespace DataServiceLayer;

public class PersonDataService : BaseDataService
{
    public PersonDataService(ImdbContext context) : base(context) { }


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
        return _context.SearchHistories.ToList();
    }

    public SearchHistory? GetSearchHistory(int id)
    {
        return _context.SearchHistories.Find(id);
    }
    public List<SearchHistory> GetSearchHistoriesByPersonId(int personId)
    {
        return _context.SearchHistories.Where(sh => sh.PersonId == personId).ToList();
    }

    // bookmarks
    public List<Bookmark> GetBookmarksByPersonId(int personId)
    {
        return _context.Bookmarks.Where(b => b.PersonId == personId).ToList();
    }

    // rating
    public List<Rating> GetRatingsByPersonId(int personId)
    {
        return _context.Ratings.Where(r => r.PersonId == personId).ToList();
    }
}

