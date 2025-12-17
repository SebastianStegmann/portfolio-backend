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
        try
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
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while finding names");
        }
    }

    // api/functions/find-coplayers?nconst=nm0705356&limit=10
    [Authorize]
    [HttpGet("find-coplayers", Name = nameof(FindCoplayers))]
    public IActionResult FindCoplayers([FromQuery] string nconst, [FromQuery] int limit = 10)
    {
        try
        {
            var personId = GetCurrentUserId();
            if (personId == null) return Unauthorized();

            if (string.IsNullOrEmpty(nconst))
            {
                return BadRequest("Nconst parameter is required");
            }
            
            var results = _dataService.FindCoplayers(nconst, (int)personId, limit);
            return Ok(results);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while finding coplayers");
        }
    }

    // api/functions/name-rating?nconst=nm0705356
    [HttpGet("name-rating", Name = nameof(GetNameRating))]
    public IActionResult GetNameRating([FromQuery] string nconst)
    {
        try
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
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while retrieving name rating");
        }
    }

    // api/functions/popular-actors?primaryTitle=Iron man&limit=10
    [HttpGet("popular-actors", Name = nameof(GetPopularActors))]
    public IActionResult GetPopularActors([FromQuery] string primaryTitle, [FromQuery] int limit = 10)
    {
        try
        {
            if (string.IsNullOrEmpty(primaryTitle))
            {
                return BadRequest("PrimaryTitle parameter is required");
            }
            
            var results = _dataService.GetPopularActors(primaryTitle, limit);
            return Ok(results);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while retrieving popular actors");
        }
    }

    // api/functions/popular-coplayers?name=Robert Downey Jr.&limit=10
    [HttpGet("popular-coplayers", Name = nameof(GetPopularCoplayers))]
    public IActionResult GetPopularCoplayers([FromQuery] string name, [FromQuery] int limit = 10)
    {
        try
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name parameter is required");
            }
            
            var results = _dataService.GetPopularCoplayers(name, limit);
            return Ok(results);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while retrieving popular coplayers");
        }
    }

    // api/functions/related-movies?tconst=tt0371746&limit=10
    [HttpGet("related-movies", Name = nameof(GetRelatedMovies))]
    public IActionResult GetRelatedMovies([FromQuery] string tconst, [FromQuery] int limit = 10)
    {
        try
        {
            if (string.IsNullOrEmpty(tconst))
            {
                return BadRequest("Tconst parameter is required");
            }
            
            var results = _dataService.GetRelatedMovies(tconst, limit);
            return Ok(results);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while retrieving related movies");
        }
    }

    // api/functions/frequent-person-words?name=Tom Hanks&limit=10
    [HttpGet("frequent-person-words", Name = nameof(GetFrequentPersonWords))]
    public IActionResult GetFrequentPersonWords([FromQuery] string name, [FromQuery] int limit = 10)
    {
        try
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name parameter is required");
            }
            
            var results = _dataService.GetFrequentPersonWords(name, limit);
            return Ok(results);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while retrieving frequent person words");
        }
    }

    // api/functions/exact-match?input=iron man&limit=10
    [HttpGet("exact-match", Name = nameof(GetExactMatch))]
    public IActionResult GetExactMatch([FromQuery] string input, [FromQuery] int limit = 10)
    {
        try
        {
            if (string.IsNullOrEmpty(input))
            {
                return BadRequest("Input parameter is required");
            }
            
            var results = _dataService.GetExactMatch(input, limit);
            return Ok(results);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while performing exact match");
        }
    }

    // api/functions/best-match?keywords=iron,man&limit=10
    [HttpGet("best-match", Name = nameof(GetBestMatch))]
    public IActionResult GetBestMatch([FromQuery] string keywords, [FromQuery] int limit = 10)
    {
        try
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
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while performing best match");
        }
    }

    // api/functions/word-to-words?keywords=iron,man
    [HttpGet("word-to-words", Name = nameof(GetWordToWords))]
    public IActionResult GetWordToWords([FromQuery] string keywords)
    {
        try
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
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while retrieving word associations");
        }
    }

    // Bookmark Endpoints

    // api/functions/bookmarks/title
    [Authorize]
    [HttpPost("bookmarks/title", Name = nameof(AddTitleBookmark))]
    public IActionResult AddTitleBookmark([FromBody] TitleBookmarkRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();
            // if (userId != request.UserId) return Unauthorized();

            if (request == null || string.IsNullOrEmpty(request.Tconst))
            {
                return BadRequest("UserId and Tconst are required");
            }
            
            var result = _dataService.AddTitleBookmark(request.UserId, request.Tconst);
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while adding title bookmark");
        }
    }

    // api/functions/bookmarks/name
    [Authorize]
    [HttpPost("bookmarks/name", Name = nameof(AddNameBookmark))]
    public IActionResult AddNameBookmark([FromBody] NameBookmarkRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();
            // if (userId != request.UserId) return Unauthorized();

            if (request == null || string.IsNullOrEmpty(request.Nconst))
            {
                return BadRequest("UserId and Nconst are required");
            }
            
            var result = _dataService.AddNameBookmark(request.UserId, request.Nconst);
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while adding name bookmark");
        }
    }

    // Delete title bookmark - DELETE: api/functions/bookmarks/title
    [Authorize]
    [HttpDelete("bookmarks/title", Name = nameof(DeleteTitleBookmark))]
    public IActionResult DeleteTitleBookmark([FromBody] TitleBookmarkRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();
            // if (userId != request.UserId) return Unauthorized();

            if (request == null || string.IsNullOrEmpty(request.Tconst))
            {
                return BadRequest("UserId and Tconst are required");
            }
            
            var result = _dataService.DeleteTitleBookmark(request.UserId, request.Tconst);
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while deleting title bookmark");
        }
    }

    // Delete name bookmark - DELETE: api/functions/bookmarks/name
    [Authorize]
    [HttpDelete("bookmarks/name", Name = nameof(DeleteNameBookmark))]
    public IActionResult DeleteNameBookmark([FromBody] NameBookmarkRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();
            // if (userId != request.UserId) return Unauthorized();

            if (request == null || string.IsNullOrEmpty(request.Nconst))
            {
                return BadRequest("UserId and Nconst are required");
            }
            
            var result = _dataService.DeleteNameBookmark(request.UserId, request.Nconst);
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while deleting name bookmark");
        }
    }

    // api/functions/string-search?searchString=batman
    [Authorize]
    [HttpGet("string-search", Name = nameof(StringSearch))]
    public IActionResult StringSearch([FromQuery] string searchString, [FromQuery] int? limit)
    {
        try
        {
            var personId = GetCurrentUserId();
            if (personId == null) return Unauthorized();

            if (string.IsNullOrEmpty(searchString))
            {
                return BadRequest("SearchString parameter is required");
            }

            // Enforce max limit
            var sanitizedLimit = Math.Min(limit ?? 50, 100); // Default 50, max 100

            var results = _dataService.StringSearch(searchString, (int)personId, sanitizedLimit);
            return Ok(results);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while performing string search");
        }
    }

    // api/functions/rate
    [Authorize]
    [HttpPost("rate", Name = nameof(RateTitle))]
    public IActionResult RateTitle([FromBody] RateTitleRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();
            // if (userId != request.PersonId) return Unauthorized();

            if (request == null || string.IsNullOrEmpty(request.Tconst))
            {
                return BadRequest("Tconst and PersonId are required");
            }

            if (request.Rating < 1 || request.Rating > 10)
            {
                return BadRequest("Rating must be between 1 and 10");
            }
            
            var result = _dataService.RateTitle(request.Tconst, (int)userId, request.Rating);
            
            if (result.Status.StartsWith("Error"))
            {
                return BadRequest(result);
            }
            
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while rating the title");
        }
    }

    // api/functions/structured-search
    [Authorize]
    [HttpGet("structured-search", Name = nameof(StructuredStringSearch))]
    public IActionResult StructuredStringSearch(
        [FromQuery] string? titleText,
        [FromQuery] string? plotText,
        [FromQuery] string? characterText,
        [FromQuery] string? personText,
        [FromQuery] int? limit)
    {
        var userId = GetCurrentUserId();
        if (userId == null) 
        {
            return Unauthorized();
        }
        
        try
        {
            var results = _dataService.StructuredStringSearch(
                titleText,
                plotText,
                characterText,
                personText,
                userId.Value,
                limit ?? 10
            );
            
            return Ok(results);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while processing your request");
        }
    }
}
