using DataServiceLayer;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.Models;
using WebServiceLayer.Models.Functions;
using Microsoft.AspNetCore.Authorization;

namespace WebServiceLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FunctionsController : BaseController<FunctionsDataService>
{
    public FunctionsController(
        FunctionsDataService dataService,
        LinkGenerator generator,
        IMapper mapper) : base(dataService, generator, mapper) { }

    // api/functions/find-names?name=radcliffe&limit=10
    [Authorize]
    [HttpGet("find-names", Name = nameof(FindNames))]
    public IActionResult FindNames([FromQuery] string name, [FromQuery] int limit = 10)
    {

        var personId = GetCurrentUserId();
        if (personId == null) return Unauthorized();

        if (string.IsNullOrEmpty(name))
        {
            return BadRequest("Name parameter is required");
        }
        
        var results = _dataService.FindNames(name, (int)personId, limit);
        return Ok(results);
    }

    // api/functions/find-coplayers?nconst=nm0705356&personId=2&limit=10
    [HttpGet("find-coplayers", Name = nameof(FindCoplayers))]
    public IActionResult FindCoplayers([FromQuery] string nconst, [FromQuery] long personId, [FromQuery] int limit = 10)
    {
        if (string.IsNullOrEmpty(nconst))
        {
            return BadRequest("Nconst parameter is required");
        }
        
        var results = _dataService.FindCoplayers(nconst, personId, limit);
        return Ok(results);
    }

    // api/functions/name-rating?nconst=nm0705356
    [HttpGet("name-rating", Name = nameof(GetNameRating))]
    public IActionResult GetNameRating([FromQuery] string nconst)
    {
        if (string.IsNullOrEmpty(nconst))
        {
            return BadRequest("Nconst parameter is required");
        }
        
        var result = _dataService.GetNameRating(nconst);
        
        if (result == null)
        {
            return NotFound($"No person found with nconst: {nconst}");
        }
        
        return Ok(result);
    }

    // // api/functions/seed-name-ratings
    // [HttpPost("seed-name-ratings", Name = nameof(SeedNameRatings))]
    // public IActionResult SeedNameRatings([FromBody] List<string> nconsts)
    // {
    //     if (nconsts == null || nconsts.Count == 0)
    //     {
    //         return BadRequest("Nconsts list is required and cannot be empty");
    //     }
    //
    //     var successCount = _dataService.SeedNameRatings(nconsts);
    //
    //     return Ok(new 
    //     { 
    //         message = "Name ratings seeding completed",
    //         totalRequested = nconsts.Count,
    //         successfullyProcessed = successCount,
    //         failed = nconsts.Count - successCount
    //     });
    // }

    // api/functions/popular-actors?primaryTitle=Iron man&limit=10
    [HttpGet("popular-actors", Name = nameof(GetPopularActors))]
    public IActionResult GetPopularActors([FromQuery] string primaryTitle, [FromQuery] int limit = 10)
    {
        if (string.IsNullOrEmpty(primaryTitle))
        {
            return BadRequest("PrimaryTitle parameter is required");
        }
        
        var results = _dataService.GetPopularActors(primaryTitle, limit);
        return Ok(results);
    }

    // api/functions/popular-coplayers?name=Robert Downey Jr.&limit=10
    [HttpGet("popular-coplayers", Name = nameof(GetPopularCoplayers))]
    public IActionResult GetPopularCoplayers([FromQuery] string name, [FromQuery] int limit = 10)
    {
        if (string.IsNullOrEmpty(name))
        {
            return BadRequest("Name parameter is required");
        }
        
        var results = _dataService.GetPopularCoplayers(name, limit);
        return Ok(results);
    }

    // api/functions/related-movies?tconst=tt0371746&limit=10
    [HttpGet("related-movies", Name = nameof(GetRelatedMovies))]
    public IActionResult GetRelatedMovies([FromQuery] string tconst, [FromQuery] int limit = 10)
    {
        if (string.IsNullOrEmpty(tconst))
        {
            return BadRequest("Tconst parameter is required");
        }
        
        var results = _dataService.GetRelatedMovies(tconst, limit);
        return Ok(results);
    }

    // api/functions/frequent-person-words?name=Tom Hanks&limit=10
    [HttpGet("frequent-person-words", Name = nameof(GetFrequentPersonWords))]
    public IActionResult GetFrequentPersonWords([FromQuery] string name, [FromQuery] int limit = 10)
    {
        if (string.IsNullOrEmpty(name))
        {
            return BadRequest("Name parameter is required");
        }
        
        var results = _dataService.GetFrequentPersonWords(name, limit);
        return Ok(results);
    }

    // api/functions/exact-match?input=iron man&limit=10
    [HttpGet("exact-match", Name = nameof(GetExactMatch))]
    public IActionResult GetExactMatch([FromQuery] string input, [FromQuery] int limit = 10)
    {
        if (string.IsNullOrEmpty(input))
        {
            return BadRequest("Input parameter is required");
        }
        
        var results = _dataService.GetExactMatch(input, limit);
        return Ok(results);
    }

    // api/functions/best-match?keywords=iron,man&limit=10
    [HttpGet("best-match", Name = nameof(GetBestMatch))]
    public IActionResult GetBestMatch([FromQuery] string keywords, [FromQuery] int limit = 10)
    {
        if (string.IsNullOrEmpty(keywords))
        {
            return BadRequest("Keywords parameter is required");
        }
        
        // Split comma-separated keywords
        var keywordList = keywords.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                   .Select(k => k.Trim())
                                   .ToList();
        
        if (keywordList.Count == 0)
        {
            return BadRequest("At least one keyword is required");
        }
        
        var results = _dataService.GetBestMatch(keywordList, limit);
        return Ok(results);
    }

    // api/functions/word-to-words?keywords=iron,man
    [HttpGet("word-to-words", Name = nameof(GetWordToWords))]
    public IActionResult GetWordToWords([FromQuery] string keywords)
    {
        if (string.IsNullOrWhiteSpace(keywords))
        {
            return BadRequest("Keywords parameter is required");
        }

        var keywordList = keywords.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                  .Select(k => k.Trim())
                                  .ToList();

        if (keywordList.Count == 0)
        {
            return BadRequest("At least one keyword is required");
        }

        var results = _dataService.GetWordToWords(keywordList);
        return Ok(results);
    }

    /* User Management Endpoints 

    // Register person - POST: api/functions/register-person
    [HttpPost("register-person", Name = nameof(RegisterPerson))]
    public IActionResult RegisterPerson([FromBody] RegisterPersonRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
        {
            return BadRequest("Email and password are required");
        }
        
        var result = _dataService.RegisterPerson(
            request.Name ?? string.Empty,
            request.Birthday,
            request.Location,
            request.Email,
            request.Password
        );
        
        if (result.Status.StartsWith("Error"))
        {
            return BadRequest(result);
        }
        
        return Ok(result);
    }

    // Update person - PUT: api/functions/update-person
    [HttpPut("update-person", Name = nameof(UpdatePerson))]
    public IActionResult UpdatePerson([FromBody] UpdatePersonRequest request)
    {
        if (request == null || request.UserId <= 0)
        {
            return BadRequest("UserId is required");
        }
        
        var result = _dataService.UpdatePerson(
            request.UserId,
            request.Name,
            request.Birthday,
            request.Location,
            request.Email
        );
        
        if (result.Status.StartsWith("Error"))
        {
            return BadRequest(result);
        }
        
        return Ok(result);
    }

    // Delete user - DELETE: api/functions/delete-user/{userId}
    [HttpDelete("delete-user/{userId}", Name = nameof(DeleteUser))]
    public IActionResult DeleteUser(long userId)
    {
        if (userId <= 0)
        {
            return BadRequest("Valid userId is required");
        }
        
        var result = _dataService.DeleteUser(userId);
        
        if (result.Status.StartsWith("Error"))
        {
            return NotFound(result);
        }
        
        return Ok(result);
    }

    */
    // Bookmark Endpoints

    // api/functions/bookmarks/title
    [Authorize]
    [HttpPost("bookmarks/title", Name = nameof(AddTitleBookmark))]
    public IActionResult AddTitleBookmark([FromBody] TitleBookmarkRequest request)
    {

        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();
        if (userId != request.UserId) return Unauthorized();

        if (request == null || string.IsNullOrEmpty(request.Tconst))
        {
            return BadRequest("UserId and Tconst are required");
        }
        
        var result = _dataService.AddTitleBookmark(request.UserId, request.Tconst);
        return Ok(result);
    }

    // api/functions/bookmarks/name
    [Authorize]
    [HttpPost("bookmarks/name", Name = nameof(AddNameBookmark))]
    public IActionResult AddNameBookmark([FromBody] NameBookmarkRequest request)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();
        if (userId != request.UserId) return Unauthorized();

        if (request == null || string.IsNullOrEmpty(request.Nconst))
        {
            return BadRequest("UserId and Nconst are required");
        }
        
        var result = _dataService.AddNameBookmark(request.UserId, request.Nconst);
        return Ok(result);
    }

    /*
    // Get user bookmarks - GET: api/functions/bookmarks/{userId}
    [HttpGet("bookmarks/{userId}", Name = nameof(GetUserBookmarks))]
    public IActionResult GetUserBookmarks(long userId)
    {
        if (userId <= 0)
        {
            return BadRequest("Valid userId is required");
        }
        
        var results = _dataService.GetUserBookmarks(userId);
        return Ok(results);
    }
    */

    // Delete title bookmark - DELETE: api/functions/bookmarks/title
    [Authorize]
    [HttpDelete("bookmarks/title", Name = nameof(DeleteTitleBookmark))]
    public IActionResult DeleteTitleBookmark([FromBody] TitleBookmarkRequest request)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();
        if (userId != request.UserId) return Unauthorized();

        if (request == null || string.IsNullOrEmpty(request.Tconst))
        {
            return BadRequest("UserId and Tconst are required");
        }
        
        var result = _dataService.DeleteTitleBookmark(request.UserId, request.Tconst);
        return Ok(result);
    }

    // Delete name bookmark - DELETE: api/functions/bookmarks/name
    [Authorize]
    [HttpDelete("bookmarks/name", Name = nameof(DeleteNameBookmark))]
    public IActionResult DeleteNameBookmark([FromBody] NameBookmarkRequest request)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();
        if (userId != request.UserId) return Unauthorized();

        if (request == null || string.IsNullOrEmpty(request.Nconst))
        {
            return BadRequest("UserId and Nconst are required");
        }
        
        var result = _dataService.DeleteNameBookmark(request.UserId, request.Nconst);
        return Ok(result);
    }
    
    

    // String search - GET: api/functions/string-search?searchString=batman&personId=1
    [HttpGet("string-search", Name = nameof(StringSearch))]
    public IActionResult StringSearch([FromQuery] string searchString, [FromQuery] long personId)
    {
        if (string.IsNullOrEmpty(searchString))
        {
            return BadRequest("SearchString parameter is required");
        }

        if (personId <= 0)
        {
            return BadRequest("Valid personId is required");
        }
        
        var results = _dataService.StringSearch(searchString, personId);
        return Ok(results);
    }

    // Rate title - POST: api/functions/rate
    [HttpPost("rate", Name = nameof(RateTitle))]
    public IActionResult RateTitle([FromBody] RateTitleRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Tconst) || request.PersonId <= 0)
        {
            return BadRequest("Tconst and PersonId are required");
        }

        if (request.Rating < 1 || request.Rating > 10)
        {
            return BadRequest("Rating must be between 1 and 10");
        }
        
        var result = _dataService.RateTitle(request.Tconst, request.PersonId, request.Rating);
        
        if (result.Status.StartsWith("Error"))
        {
            return BadRequest(result);
        }
        
        return Ok(result);
    }

    // Structured string search - POST: api/functions/structured-search
    [HttpPost("structured-search", Name = nameof(StructuredStringSearch))]
    public IActionResult StructuredStringSearch([FromBody] StructuredSearchRequest request)
    {
        if (request == null || request.PersonId <= 0)
        {
            return BadRequest("PersonId is required");
        }
        
        var limit = request.Limit ?? 10;
        var results = _dataService.StructuredStringSearch(
            request.TitleText,
            request.PlotText,
            request.CharacterText,
            request.PersonText,
            request.PersonId,
            limit
        );
        
        return Ok(results);
    }
}

public class RateTitleRequest
{
    public string Tconst { get; set; } = null!;
    public long PersonId { get; set; }
    public int Rating { get; set; }
}

public class StructuredSearchRequest
{
    public string? TitleText { get; set; }
    public string? PlotText { get; set; }
    public string? CharacterText { get; set; }
    public string? PersonText { get; set; }
    public long PersonId { get; set; }
    public int? Limit { get; set; }
}
