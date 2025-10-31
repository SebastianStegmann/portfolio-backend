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
        var names = _dataService
            .GetNames(queryParams.Page, queryParams.PageSize)
            .Select(x => CreateNameListModel(x));

        var numOfItems = _dataService.GetNamesCount();

        var result = CreatePaging(nameof(GetNames), names, numOfItems, queryParams);

        return Ok(result);
    }

    // Getting one actor by nconst - GET: api/names/{nconst}
    [HttpGet("{nconst}", Name = nameof(GetName))]
    public IActionResult GetName(string nconst)
    {
        var name = _dataService.GetName(nconst);
        if (name == null) return NotFound();
        NameModel model = CreateNameModel(name);

        return Ok(model);
    }

    // Getting the movies that the Actor is known for - GET: api/names/{nconst}/knownfor
    [HttpGet("{nconst}/knownfor", Name = nameof(GetKnownForTitles))]
    public IActionResult GetKnownForTitles(string nconst)
    {
        var titles = _dataService.GetKnownForTitles(nconst);
        if (titles == null || titles.Count == 0) return NotFound();

        // Map to TitleModel with URLs
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

    //object-object mapping

    // Information shown when listing all the names
    private NameListModel CreateNameListModel(DataServiceLayer.Models.Name.NameBasics name)
    {
        var model = _mapper.Map<NameListModel>(name);
        model.URL = GetUrl(nameof(GetName), new { Nconst = name.Nconst.Trim() });

        // Check database directly instead of navigation property
        if (_dataService.HasKnownForTitles(name.Nconst.Trim()))
        {
            model.KnownForURL = GetUrl(nameof(GetKnownForTitles), new { nconst = name.Nconst.Trim() });
        }

        return model;
    }

    // Information shown when clicking on a specific name
    private NameModel CreateNameModel(DataServiceLayer.Models.Name.NameBasics name)
    {
        var model = _mapper.Map<NameModel>(name);
        model.URL = GetUrl(nameof(GetName), new { Nconst = name.Nconst.Trim() });

        // Check database directly instead of navigation property
        if (_dataService.HasKnownForTitles(name.Nconst.Trim()))
        {
            model.KnownForURL = GetUrl(nameof(GetKnownForTitles), new { nconst = name.Nconst.Trim() });
        }

        return model;
    }
}
