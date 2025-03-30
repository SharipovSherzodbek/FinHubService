using Microsoft.AspNetCore.Mvc;

namespace FinhubService.Controllers
{
  public class HomeController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
  }
}
