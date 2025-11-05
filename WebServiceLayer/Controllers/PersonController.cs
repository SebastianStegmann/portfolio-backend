using DataServiceLayer;
using DataServiceLayer.Models.Person;
using DataServiceLayer.Models.Title;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
// using System.Security.Claims;
using System.Xml.Linq;
using WebServiceLayer.Models;

namespace WebServiceLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : BaseController<PersonDataService>
{
    public PersonController(
        PersonDataService dataService,
        LinkGenerator generator,
        IMapper mapper) : base(dataService, generator, mapper) { }

    [Authorize]
    [HttpGet(Name = nameof(GetLoggedInPerson))]
    public IActionResult GetLoggedInPerson()
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var person = _dataService.GetPerson(userId.Value);
        if (person == null) return NotFound();

        var model = CreatePersonListModel(person);
        return Ok(model);
    }

    // api/person/{id}
    [HttpGet("{id}", Name = nameof(GetPerson))]
    public IActionResult GetPerson(int id)
    {
        var person = _dataService.GetPerson(id);
        if (person == null) return NotFound();
        PersonModel model = CreatePersonModel(person);
        return Ok(model);
    }

    // api/person/searchhistory
    [Authorize]
    [HttpGet("searchhistory", Name = nameof(GetSearchHistory))]
    public IActionResult GetSearchHistory()
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var searchHistory = _dataService.GetSearchHistoriesByPersonId(userId.Value);
        return Ok(searchHistory);
    }

    [Authorize]
    [HttpGet("bookmarks", Name = nameof(GetPersonBookmarks))]
    public IActionResult GetPersonBookmarks()
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var bookmarks = _dataService.GetBookmarksByPersonId(userId.Value);

        var bookmarkModels = bookmarks.Select(b => new BookmarkModel
        {
            Tconst = b.Tconst.Trim(),
            CreatedAt = b.CreatedAt,
            TitleURL = GetUrl("GetTitle", new { Tconst = b.Tconst.Trim() })
        }).ToList();

        return Ok(bookmarkModels);

    }

    [Authorize]
    [HttpGet("name_bookmarks", Name = nameof(GetPersonNameBookmarks))]
    public IActionResult GetPersonNameBookmarks()
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var bookmarks = _dataService.GetNameBookmarksByPersonId(userId.Value);

        var bookmarkModels = bookmarks.Select(b => new NameBookmarkModel
        {
            Nconst = b.Nconst.Trim(),
            CreatedAt = b.CreatedAt,
            TitleURL = GetUrl("GetTitle", new { Tconst = b.Nconst.Trim() })
        }).ToList();

        return Ok(bookmarkModels);
    }

    //  api/person/ratings
    [Authorize]
    [HttpGet("ratings", Name = nameof(GetPersonRatings))]
    public IActionResult GetPersonRatings()
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var ratings = _dataService.GetRatingsByPersonId(userId.Value);

        var ratingsModels = ratings.Select(b => new RatingsModel
        {
            Tconst = b.Tconst.Trim(),
            CreatedAt = b.CreatedAt,
            TitleURL = GetUrl("GetTitle", new { Tconst = b.Tconst.Trim() })
        }).ToList();

        return Ok(ratingsModels);
    }



    // object-object mapping
    // Information shown when listing all the persons
    private PersonListModel CreatePersonListModel(DataServiceLayer.Models.Person.Person person)
    {
        var model = _mapper.Map<PersonListModel>(person);
        model.URL = GetUrl(nameof(GetPerson), new { id = person.Id });

        return model;
    }

    // Information shown when getting a single person
    private PersonModel CreatePersonModel(DataServiceLayer.Models.Person.Person person)
    {
        var model = _mapper.Map<PersonModel>(person);
        model.URL = GetUrl(nameof(GetPerson), new { id = person.Id });

        if (person.Search != null && person.Search.Any())
        {
            model.SearchURL = GetUrl(nameof(GetSearchHistory), new { });
        }

        if (person.Bookmark != null && person.Bookmark.Any())
        {
            model.BookmarkURL = GetUrl(nameof(GetPersonBookmarks), new { });
        }

        if (person.IndividualRating != null && person.IndividualRating.Any())
        {
            model.IndividualRatingURL = GetUrl(nameof(GetPersonRatings), new { });
        }

        return model;
    }
}
