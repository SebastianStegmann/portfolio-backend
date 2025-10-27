using DataServiceLayer;
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
        var titleModels = titles.Select(title => new TitleModel
        {
            URL = GetUrl("GetTitle", new { Tconst = title.Tconst.Trim() }),
            PrimaryTitle = title.PrimaryTitle,
            TitleType = title.TitleType,
            ReleaseDate = title.ReleaseDate,
            RuntimeMinutes = title.RuntimeMinutes,
            Poster = title.Poster
        });
        return Ok(titleModels);
    }

    // Getting the professions for one actor - GET: api/names/{nconst}/professions
    [HttpGet("{nconst}/professions")]
    public IActionResult GetNameProfessions(string nconst)
    {
        var professions = _dataService.GetNameProfessions(nconst);
        if (professions == null || professions.Count == 0) return NotFound();
        return Ok(professions);
    }

    // Getting all names with a specific profession - GET: api/names/byprofession/{professionId}
    [HttpGet("byprofession/{professionId}")]
    public IActionResult GetNamesByProfession(int professionId)
    {
        var names = _dataService.GetNamesByProfession(professionId);
        if (names == null || names.Count == 0) return NotFound();
        return Ok(names);
    }

    // Getting all names known for a specific title - GET: api/names/knownfortitle/{tconst}
    [HttpGet("knownfortitle/{tconst}")]
    public IActionResult GetNamesKnownForTitle(string tconst)
    {
        var names = _dataService.GetNamesKnownForTitle(tconst);
        if (names == null || names.Count == 0) return NotFound();
        return Ok(names);
    }
    // Getting the roles of a name in a specific title - GET: api/names/{nconst}/roles/title/{tconst}
    [HttpGet("{nconst}/roles/title/{tconst}")]
    public IActionResult GetNameRolesInTitle(string nconst, string tconst)
    {
        var roles = _dataService.GetNameRolesInTitle(nconst, tconst);
        if (roles == null || roles.Count == 0) return NotFound();
        return Ok(roles);
    }

    // Getting all roles for a name - GET: api/names/{nconst}/roles
    [HttpGet("{nconst}/roles")]
    public IActionResult GetAllRolesForName(string nconst)
    {
        var roles = _dataService.GetAllRolesForName(nconst);
        if (roles == null || roles.Count == 0) return NotFound();
        return Ok(roles);
    }

    // Getting all titles for a name with a specific role - GET: api/names/{nconst}/titles/role/{roleId}
    [HttpGet("{nconst}/titles/role/{roleId}")]
    public IActionResult GetTitlesByNameAndRole(string nconst, int roleId)
    {
        var titles = _dataService.GetTitlesByNameAndRole(nconst, roleId);
        if (titles == null || titles.Count == 0) return NotFound();
        return Ok(titles);
    }

    // Getting all names with a specific role in a title - GET: api/names/title/{tconst}/role/{roleId}
    [HttpGet("title/{tconst}/role/{roleId}")]
    public IActionResult GetNamesByRoleInTitle(string tconst, int roleId)
    {
        var names = _dataService.GetNamesByRoleInTitle(tconst, roleId);
        if (names == null || names.Count == 0) return NotFound();
        return Ok(names);
    }


    //object-object mapping

    // Information shown when listing all the names
    private NameListModel CreateNameListModel(DataServiceLayer.Models.NameBasics.NameBasics name)
    {
        var model = _mapper.Map<NameListModel>(name);
        model.URL = GetUrl(nameof(GetName), new { Nconst = name.Nconst.Trim() });

        // Only generate KnownForURL if the actor has titles
        if (name.Titles != null && name.Titles.Any())
        {
            model.KnownForURL = GetUrl(nameof(GetKnownForTitles), new { nconst = name.Nconst.Trim() });
        }

        return model;
    }

    // Information shown when clicking on a specific name
    private NameModel CreateNameModel(DataServiceLayer.Models.NameBasics.NameBasics name)
    {
        var model = _mapper.Map<NameModel>(name);
        model.URL = GetUrl(nameof(GetName), new { Nconst = name.Nconst.Trim() });

        // Only generate KnownForURL if the actor has titles
        if (name.Titles != null && name.Titles.Any())
        {
            model.KnownForURL = GetUrl(nameof(GetKnownForTitles), new { nconst = name.Nconst.Trim() });
        }
        else
        {
            model.KnownForURL = null;  // Explicitly set to null if no titles
        }

        return model;
    }
}
