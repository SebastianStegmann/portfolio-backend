using DataServiceLayer;
using Microsoft.AspNetCore.Mvc;

namespace WebServiceLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NamesController : Controller
{
    private readonly DataService _dataService;

    // Getting all actors
    public NamesController(DataService dataService)
    {
        _dataService = dataService;
    }

    [HttpGet]
    public IActionResult GetNames()
    {
        return Ok(_dataService.GetNames());
    }

    // Getting one actor by nconst
    [HttpGet("{nconst}")]
    public IActionResult GetNames(string nconst)
    {
        var name = _dataService.GetName(nconst);
        if (name == null) return NotFound();
        return Ok(name);
    }

    // Getting the movies that the Actor is known for
    [HttpGet("{nconst}/knownfor")]
    public IActionResult GetKnownForTitles(string nconst)
    {
        var titles = _dataService.GetKnownForTitles(nconst);
        if (titles == null || titles.Count == 0) return NotFound();
        return Ok(titles);
    }

    // Getting the professions for one actor
    [HttpGet("{nconst}/professions")]
    public IActionResult GetNameProfessions(string nconst)
    {
        var professions = _dataService.GetNameProfessions(nconst);
        if (professions == null || professions.Count == 0) return NotFound();
        return Ok(professions);
    }

    // Getting all names with a specific profession
    [HttpGet("byprofession/{professionId}")]
    public IActionResult GetNamesByProfession(int professionId)
    {
        var names = _dataService.GetNamesByProfession(professionId);
        if (names == null || names.Count == 0) return NotFound();
        return Ok(names);
    }

    // Getting all names known for a specific title
    [HttpGet("knownfortitle/{tconst}")]
    public IActionResult GetNamesKnownForTitle(string tconst)
    {
        var names = _dataService.GetNamesKnownForTitle(tconst);
        if (names == null || names.Count == 0) return NotFound();
        return Ok(names);
    }
}
