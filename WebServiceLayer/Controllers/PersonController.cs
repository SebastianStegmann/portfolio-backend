using DataServiceLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebServiceLayer;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly PersonDataService _personDataService;

    public PersonController(PersonDataService personDataService)
    {
        _personDataService = personDataService;
    }

    // Get person by ID
    [HttpGet("{id}")]
    public IActionResult GetPerson(int id)
    {
        var person = _personDataService.GetPerson(id);
        if (person == null) return NotFound();
        return Ok(person);
    }

    // Get search history for a specific person
    [Authorize]
    [HttpGet("{personId}/searchhistory")]
    public IActionResult GetPersonSearchHistory(int personId)
    {
        var searchHistory = _personDataService.GetSearchHistoriesByPersonId(personId);
        return Ok(searchHistory);
    }

    // Get bookmark from a specific person
    [HttpGet("{personId}/bookmarks")]
    public IActionResult GetPersonBookmarks(int personId)
    {
        var bookmarks = _personDataService.GetBookmarksByPersonId(personId);
        return Ok(bookmarks);
    }

    // Get ratings for a specific person
    [HttpGet("{personId}/ratings")]
    public IActionResult GetPersonRatings(int personId)
    {
        var ratings = _personDataService.GetRatingsByPersonId(personId);
        return Ok(ratings);
    }
}
