using DataServiceLayer;
using DataServiceLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebServiceLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenreController : ControllerBase
{
    private readonly TitleDataService _dataService;

    public GenreController(TitleDataService dataService)
    {
        _dataService = dataService;
    }

    // Getting all genres
    [HttpGet]
    public IActionResult GetAllGenres()
    {
        return Ok(_dataService.GetAllGenres());
    }
}