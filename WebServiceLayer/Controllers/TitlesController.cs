using DataServiceLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebServiceLayer;

[ApiController]
[Route("api/[controller]")]
public class TitlesController : ControllerBase
{
  private readonly TitleDataService _dataService;

  public TitlesController(TitleDataService dataService)
  {
    _dataService = dataService;
  }

  [HttpGet]
  public IActionResult GetTitles()
  {
    return Ok(_dataService.GetTitles());
  }

  [HttpGet("{tconst}")]
  public IActionResult GetTitles(string tconst)
  {
    var title = _dataService.GetTitle(tconst);
    if (title == null) return NotFound();
    return Ok(title);
  }

  // awards endpoint
  [HttpGet("{tconst}/awards")]
  public IActionResult GetAwardsByTconst(string tconst)
  {
    var awards = _dataService.GetAwardsByTconst(tconst);
    return Ok(awards);
  }

  // overall rating endpoint
  [HttpGet("{tconst}/overallrating")]
  public IActionResult GetOverallRatings(string tconst)
  {
    var overallRatings = _dataService.GetOverallRatings(tconst);
    return Ok(overallRatings);
  }
}
