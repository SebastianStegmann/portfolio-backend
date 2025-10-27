using DataServiceLayer;
using DataServiceLayer.Models.TitleBasics;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.Models;

namespace WebServiceLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TitlesController : BaseController<TitleDataService>
{
  public TitlesController(
      TitleDataService dataService,
      LinkGenerator generator,
      IMapper mapper) : base(dataService, generator, mapper) { }

  // Getting all titles - GET: api/titles
  [HttpGet(Name = nameof(GetTitles))]
  public IActionResult GetTitles([FromQuery] QueryParams queryParams)
  {
    queryParams.PageSize = Math.Min(queryParams.PageSize, 10);
    var titles = _dataService
        .GetTitles(queryParams.Page, queryParams.PageSize)
        .Select(x => CreateTitleModel(x));

    var numOfItems = _dataService.GetTitlesCount();

    var result = CreatePaging(nameof(GetTitles), titles, numOfItems, queryParams);

    return Ok(result);
  }

  // Getting one title by tconst - GET: api/names/{tconst}
  [HttpGet("{tconst}", Name = nameof(GetTitle))]
  public IActionResult GetTitle(string tconst)
  {
    var title = _dataService.GetTitle(tconst);
    if (title == null) return NotFound();
    TitleModel model = CreateTitleModel(title);

    return Ok(model);
  }

    // Getting all names known for a specific title - GET: api/{tconst}/allactors
    [HttpGet("titles/{tconst}/allactors", Name = nameof(GetActorsForTitle))]
    public IActionResult GetActorsForTitle(string tconst)
    {
        var names = _dataService.GetActorsForTitle(tconst);
        if (names == null || names.Count == 0) return NotFound();

        //Map tp NameLIstModel with URLs
        var nameModel = names.Select(name => new NameListModel
        {
            URL = GetUrl("GetName", new { Nconst = name.Nconst.Trim() }),
            Name = name.Name,
            KnownForURL = GetUrl("GetKnownForTitles", new { Nconst = name.Nconst.Trim() })
        });

        return Ok(nameModel);
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

    //object-object mapping
    private TitleModel CreateTitleModel(DataServiceLayer.Models.TitleBasics.TitleBasics title)
    {
        var model = _mapper.Map<TitleModel>(title);
        model.URL = GetUrl(nameof(GetTitle), new { Tconst = title.Tconst.Trim() });
        return model;
    }
}
