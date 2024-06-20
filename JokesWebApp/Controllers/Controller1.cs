using Microsoft.AspNetCore.Mvc;

namespace JokesWebApp.Controllers
{
    public class Controller1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
