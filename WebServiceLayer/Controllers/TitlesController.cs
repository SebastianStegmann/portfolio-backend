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
      IMapper mapper) : base(dataService, generator, mapper){}

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
    if ( title == null ) return NotFound();
        TitleModel model = CreateTitleModel(title);

    return Ok(model);
  }

    //object-object mapping
    private TitleModel CreateTitleModel(DataServiceLayer.Models.TitleBasics.TitleBasics title)
    {
        var model = _mapper.Map<TitleModel>(title);
        model.URL = GetUrl(nameof(GetTitle), new { Tconst = title.Tconst.Trim() });
        return model;
    }
}
