using DataServiceLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebServiceLayer;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly DataService _dataService;

    public PersonController(DataService dataService)
    {
        _dataService = dataService;
    }

    // Get person by ID
    [HttpGet("{id}")]
    public IActionResult GetPerson(int id)
    {
        var person = _dataService.GetPerson(id);
        if (person == null) return NotFound();
        return Ok(person);
    }

    // Get search history for a specific person
    [HttpGet("{personId}/searchhistory")]
    public IActionResult GetPersonSearchHistory(int personId)
    {
        var searchHistory = _dataService.GetSearchHistoriesByPersonId(personId);
        return Ok(searchHistory);
    }

    // Get bookmark from a specific person
    [HttpGet("{personId}/bookmarks")]
    public IActionResult GetPersonBookmarks(int personId)
    {
        // Assuming a method GetBookmarksByPersonId exists in DataService
        var bookmarks = _dataService.GetBookmarksByPersonId(personId);
        return Ok(bookmarks);
    }

    // Get ratings for a specific person
    [HttpGet("{personId}/ratings")]
    public IActionResult GetPersonRatings(int personId)
    {
        var ratings = _dataService.GetRatingsByPersonId(personId);
        return Ok(ratings);
    }
}
