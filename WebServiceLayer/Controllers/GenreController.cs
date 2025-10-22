using DataServiceLayer;
using Microsoft.AspNetCore.Mvc;

namespace WebServiceLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenreController : ControllerBase
{
    private readonly DataService _dataService;

    public GenreController(DataService dataService)
    {
        _dataService = dataService;
    }

    // Getting all professions
    [HttpGet]
    public IActionResult GetAllGenres()
    {
        return Ok(_dataService.GetAllGenres());
    }
}