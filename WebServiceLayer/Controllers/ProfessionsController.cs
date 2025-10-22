using DataServiceLayer;
using Microsoft.AspNetCore.Mvc;

namespace WebServiceLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfessionsController : ControllerBase
{
    private readonly NameDataService _dataService;

    public ProfessionsController(NameDataService dataService)
    {
        _dataService = dataService;
    }

    // Getting all professions
    [HttpGet]
    public IActionResult GetAllProfessions()
    {
        return Ok(_dataService.GetAllProfessions());
    }
}