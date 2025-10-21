using DataServiceLayer;
using Microsoft.AspNetCore.Mvc;

namespace WebServiceLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfessionsController : ControllerBase
{
    private readonly DataService _dataService;

    public ProfessionsController(DataService dataService)
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