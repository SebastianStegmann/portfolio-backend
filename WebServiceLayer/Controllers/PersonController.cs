using DataServiceLayer;
using DataServiceLayer.Models.Title;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Xml.Linq;
using WebServiceLayer.Models;

namespace WebServiceLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : BaseController<PersonDataService>
{
    private const bool USE_DEV_MODE = true; // Set to false for production

    public PersonController(
        PersonDataService dataService,
        LinkGenerator generator,
        IMapper mapper) : base(dataService, generator, mapper) { }

    private int? GetCurrentUserId()
    {
        // PRODUCTION MODE: Get user ID from JWT token
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim != null && int.TryParse(userIdClaim, out int userId))
        {
            return userId;
        }

        return null;
    }

    // Get logged-in person's information OR all persons in dev mode - GET: api/person
    [HttpGet(Name = nameof(GetLoggedInPerson))]
    public IActionResult GetLoggedInPerson()
    {
        // DEVELOPMENT MODE: Return all persons
        if (USE_DEV_MODE)
        {
            var allPersons = _dataService.GetAllPersons();
            var models = allPersons.Select(p => CreatePersonListModel(p)).ToList();
            return Ok(models);
        }

        // PRODUCTION MODE: Return only the logged-in user's info
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var person = _dataService.GetPerson(userId.Value);
        if (person == null) return NotFound();

        var model = CreatePersonListModel(person);
        return Ok(model);
    }

    // Get person by ID (admin) - GET: api/person/{id}
    [HttpGet("{id}", Name = nameof(GetPerson))]
    public IActionResult GetPerson(int id)
    {
        var person = _dataService.GetPerson(id);
        if (person == null) return NotFound();
        PersonModel model = CreatePersonModel(person);
        return Ok(person);
    }

    // Get search history for the logged-in person
    [HttpGet("searchhistory")]
    public IActionResult GetPersonSearchHistory()
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var searchHistory = _dataService.GetSearchHistoriesByPersonId(userId.Value);
        return Ok(searchHistory);
    }

    // Get bookmarks for the logged-in person
    [HttpGet("bookmarks")]
    public IActionResult GetPersonBookmarks()
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var bookmarks = _dataService.GetBookmarksByPersonId(userId.Value);
        return Ok(bookmarks);
    }

    // Get ratings for the logged-in person
    [HttpGet("ratings")]
    public IActionResult GetPersonRatings()
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var ratings = _dataService.GetRatingsByPersonId(userId.Value);
        return Ok(ratings);
    }
    private PersonListModel CreatePersonListModel(DataServiceLayer.Models.Person.Person person)
    {
        var model = _mapper.Map<PersonListModel>(person);
        model.URL = GetUrl(nameof(GetPerson), new { id = person.Id });

        return model;
    }

    private PersonModel CreatePersonModel(DataServiceLayer.Models.Person.Person person)
    {
        var model = _mapper.Map<PersonModel>(person);
        model.URL = GetUrl(nameof(GetPerson), new { id = person.Id });

        return model;
    }
}
