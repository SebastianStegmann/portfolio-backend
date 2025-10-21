using DataServiceLayer;
using Microsoft.AspNetCore.Mvc;

namespace WebServiceLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NamesController : Controller
{
    private readonly DataService _dataService;

    public NamesController(DataService dataService)
    {
        _dataService = dataService;
    }

    [HttpGet]
    public IActionResult GetNames()
    {
        return Ok(_dataService.GetNames());
    }

    [HttpGet("{nconst}")]
    public IActionResult GetNames(string nconst)
    {
        var name = _dataService.GetName(nconst);
        if (name == null) return NotFound();
        return Ok(name);
    }
}
