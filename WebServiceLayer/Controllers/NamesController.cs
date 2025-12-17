using DataServiceLayer;
using DataServiceLayer.Models.Title;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.Models;

namespace WebServiceLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NamesController : BaseController<NameDataService>
{
  public NamesController(
      NameDataService dataService,
      LinkGenerator generator,
      IMapper mapper) : base(dataService, generator, mapper){}

  // Getting all actors - GET: api/names
  [HttpGet(Name = nameof(GetNames))]
  public IActionResult GetNames([FromQuery] QueryParams queryParams)
  {
    try
    {
      var names = _dataService
        .GetNames(queryParams.Page, queryParams.PageSize)
        .Select(x => CreateNameListModel(x));

      var numOfItems = _dataService.GetNamesCount();
      var result = CreatePaging(nameof(GetNames), names, numOfItems, queryParams);

      return Ok(result);
    }
    catch (Exception)
    {
      return StatusCode(500, "An error occurred while retrieving names");
    }
  }

  [HttpGet("{nconst}", Name = nameof(GetName))]
  public IActionResult GetName(string nconst)
  {
    try
    {
      var name = _dataService.GetName(nconst);
      if (name == null) return NotFound();

      NameModel model = CreateNameModel(name);
      return Ok(model);
    }
    catch (Exception)
    {
      return StatusCode(500, "An error occurred while retrieving the name");
    }
  }

  [HttpGet("{nconst}/titles", Name = nameof(GetTitlesByNconst))]
  public IActionResult GetTitlesByNconst(string nconst)
  {
    try
    {
      var titles = _dataService.GetTitlesByNconst(nconst);
      if (titles == null || titles.Count == 0) return NotFound();

      var titleListModels = titles.Select(title => new TitleListModel
          {
          URL = GetUrl("GetTitle", new { Tconst = title.Tconst.Trim() }),
          PrimaryTitle = title.PrimaryTitle,
          TitleType = title.TitleType,
          ReleaseDate = title.ReleaseDate,
          Poster = title.Poster,
          AllCastURL = GetUrl("GetCastForTitle", new { tconst = title.Tconst.Trim() })
          });

      return Ok(titleListModels);
    }
    catch (Exception)
    {
      return StatusCode(500, "An error occurred while retrieving titles");
    }
  }

  [HttpGet("{nconst}/profession", Name = nameof(GetNameProfession))]
  public IActionResult GetNameProfession(string nconst)
  {
    try
    {
      var professions = _dataService.GetNameProfessions(nconst);

      if (!professions.Any())
        return NotFound();

      return Ok(professions);
    }
    catch (Exception)
    {
      return StatusCode(500, "An error occurred while retrieving professions");
    }
  }

  [HttpGet("{nconst}/knownfor", Name = nameof(GetKnownForTitles))]
  public IActionResult GetKnownForTitles(string nconst)
  {
    try
    {
      var titles = _dataService.GetKnownForTitles(nconst);
      if (titles == null || titles.Count == 0) return NotFound();

      var titleListModels = titles.Select(title => new TitleListModel
          {
          URL = GetUrl("GetTitle", new { Tconst = title.Tconst.Trim() }),
          PrimaryTitle = title.PrimaryTitle,
          TitleType = title.TitleType,
          ReleaseDate = title.ReleaseDate,
          Poster = title.Poster,
          AllCastURL = GetUrl("GetCastForTitle", new { tconst = title.Tconst.Trim() })
          });

      return Ok(titleListModels);
    }
    catch (Exception)
    {
      return StatusCode(500, "An error occurred while retrieving known for titles");
    }
  }
  //object-object mapping

  private NameListModel CreateNameListModel(DataServiceLayer.Models.Name.NameBasics name)
  {
    var model = _mapper.Map<NameListModel>(name);
    model.URL = GetUrl(nameof(GetName), new { Nconst = name.Nconst.Trim() });

    if (_dataService.HasKnownForTitles(name.Nconst.Trim()))
    {
      model.KnownForURL = GetUrl(nameof(GetKnownForTitles), new { nconst = name.Nconst.Trim() });
    }

    return model;
  }

  private NameModel CreateNameModel(DataServiceLayer.Models.Name.NameBasics name)
  {
    var model = _mapper.Map<NameModel>(name);
    model.URL = GetUrl(nameof(GetName), new { Nconst = name.Nconst.Trim() });

    if (_dataService.HasKnownForTitles(name.Nconst.Trim()))
    {
      model.KnownForURL = GetUrl(nameof(GetKnownForTitles), new { nconst = name.Nconst.Trim() });
    }

    return model;
  }
}
