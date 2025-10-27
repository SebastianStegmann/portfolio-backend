using DataServiceLayer.Models;
using DataServiceLayer.Models.Name;
using DataServiceLayer.Models.Person;
namespace DataServiceLayer;

public class PersonDataService : BaseDataService
{
    public PersonDataService(ImdbContext context) : base(context) { }


    // person
    public List<Person> GetPerson()
    {
        return _context.Persons.ToList();
    }

    public Person? GetPerson(int Id)
    {
        return _context.Persons.Find(Id);
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
    public List<IndividualRating> GetRatingsByPersonId(int personId)
    {
        return _context.IndividualRatings.Where(r => r.PersonId == personId).ToList();
    }

    public async Task AddPersonAsync(Person person)
    {
        _context.Persons.Add(person);
        await _context.SaveChangesAsync();
    }
}

