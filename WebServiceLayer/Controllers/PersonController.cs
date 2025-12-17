using DataServiceLayer;
using DataServiceLayer.Models.Person;
using DataServiceLayer.Models.Title;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    try
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();
        var person = _dataService.GetPerson(userId.Value);
        if (person == null) return NotFound();
        var model = CreatePersonListModel(person);
        return Ok(model);
    }
    catch (Exception)
    {
        return StatusCode(500, "An error occurred while retrieving person information");
    }
}

// api/person/searchhistory
[Authorize]
[HttpGet("searchhistory", Name = nameof(GetSearchHistory))]
public IActionResult GetSearchHistory()
{
    try
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();
        var searchHistory = _dataService.GetSearchHistoriesByPersonId(userId.Value);
        return Ok(searchHistory);
    }
    catch (Exception)
    {
        return StatusCode(500, "An error occurred while retrieving search history");
    }
}

[Authorize]
[HttpGet("bookmarks", Name = nameof(GetPersonBookmarks))]
public IActionResult GetPersonBookmarks()
{
    try
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
    catch (Exception)
    {
        return StatusCode(500, "An error occurred while retrieving bookmarks");
    }
}

[Authorize]
[HttpGet("name_bookmarks", Name = nameof(GetPersonNameBookmarks))]
public IActionResult GetPersonNameBookmarks()
{
    try
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
    catch (Exception)
    {
        return StatusCode(500, "An error occurred while retrieving name bookmarks");
    }
}

//  api/person/ratings
[Authorize]
[HttpGet("ratings", Name = nameof(GetPersonRatings))]
public IActionResult GetPersonRatings()
{
    try
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();
        var ratings = _dataService.GetRatingsByPersonId(userId.Value);
        var ratingsModels = ratings.Select(b => new RatingsModel
        {
            Tconst = b.Tconst.Trim(),
            RatingValue = b.RatingValue,
            CreatedAt = b.CreatedAt,
            TitleURL = GetUrl("GetTitle", new { Tconst = b.Tconst.Trim() })
        }).ToList();
        return Ok(ratingsModels);
    }
    catch (Exception)
    {
        return StatusCode(500, "An error occurred while retrieving ratings");
    }
}

[HttpPut("profile")]  
[Authorize]
public IActionResult UpdateProfile([FromBody] UpdateProfileDto profileData)
{
    try
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();
        var person = _dataService.GetPerson(userId.Value);
        if (person == null) return NotFound();
        
        // Only update if values are provided
        if (!string.IsNullOrEmpty(profileData.Name))
            person.Name = profileData.Name;
        
        if (!string.IsNullOrEmpty(profileData.Email))
            person.Email = profileData.Email;
        
        // Only update birthday if explicitly provided (not null)
        if (profileData.Birthday.HasValue)
        {
            person.Birthday = profileData.Birthday;
        }
        
        if (!string.IsNullOrEmpty(profileData.Location))
            person.Location = profileData.Location;
        
        _dataService.UpdatePerson(person); 
        return Ok(new { message = "Profile updated successfully" });
    }
    catch (Exception)
    {
        return StatusCode(500, "An error occurred while updating profile");
    }
}

    // object-object mapping
    // Information shown when listing all the persons
    private PersonListModel CreatePersonListModel(DataServiceLayer.Models.Person.Person person)
    {
        var model = _mapper.Map<PersonListModel>(person);
        model.URL = GetUrl(nameof(GetLoggedInPerson), new {});

        return model;
    }

    // Information shown when getting a single person
    private PersonModel CreatePersonModel(DataServiceLayer.Models.Person.Person person)
    {
        var model = _mapper.Map<PersonModel>(person);
        model.URL = GetUrl(nameof(GetLoggedInPerson), new {});

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

