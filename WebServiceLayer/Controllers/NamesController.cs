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
        return Ok(_dataService.GetTitles());
    }

    [HttpGet("{nconst}")]
    public IActionResult GetNames(string nconst)
    {
        var title = _dataService.GetName(nconst);
        if (title == null) return NotFound();
        return Ok(title);
    }
}
