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
    if ( title == null ) return NotFound();
    return Ok(title);
  }
}
