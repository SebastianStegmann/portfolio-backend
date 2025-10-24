using Microsoft.AspNetCore.Mvc;

namespace WebServiceLayer.Controllers
{
    public class BaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
