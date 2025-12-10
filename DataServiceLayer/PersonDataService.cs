using DataServiceLayer.Models;
using DataServiceLayer.Models.Name;
using DataServiceLayer.Models.Person;
using Microsoft.EntityFrameworkCore;
namespace DataServiceLayer;

public class PersonDataService : BaseDataService
{
    public PersonDataService(ImdbContext context) : base(context) { }

    // Get all persons (for development/admin)
    public List<Person> GetAllPersons()
    {
        return _context.Persons.ToList();
    }

    // Get a single person by ID
    public Person? GetPerson(int Id)
    {
        return _context.Persons
            .Include(p => p.Search)
            .Include(p => p.IndividualRating)
            .Include(p => p.Bookmark)
            .FirstOrDefault(p => p.Id == Id);
    }

    // search history
    public List<SearchHistory> GetSearchHistoriesByPersonId(int personId)
    {
        return _context.SearchHistories.Where(sh => sh.PersonId == personId).ToList();
    }

    // bookmarks
    public List<Bookmark> GetBookmarksByPersonId(int personId)
    {
        return _context.Bookmarks.Where(b => b.PersonId == personId).ToList();
    }


    // bookmarks
    public List<NameBookmark> GetNameBookmarksByPersonId(int personId)
    {
        return _context.NameBookmarks.Where(b => b.PersonId == personId).ToList();
    }

    // rating
    public List<IndividualRating> GetRatingsByPersonId(int personId)
    {
        return _context.IndividualRatings.Where(r => r.PersonId == personId).ToList();
    }

    public void UpdatePerson(Person person)
    {
        if (person.Birthday.HasValue)
        {
            person.Birthday = DateTime.SpecifyKind(person.Birthday.Value, DateTimeKind.Utc);
        }
        
        if (person.LastLogin.HasValue)
        {
            person.LastLogin = DateTime.SpecifyKind(person.LastLogin.Value, DateTimeKind.Utc);
        }
        
        // CreatedAt is required, so always convert it
        person.CreatedAt = DateTime.SpecifyKind(person.CreatedAt, DateTimeKind.Utc);

        _context.Persons.Update(person);
        _context.SaveChanges();
    }
}

