using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Quizer.Websiite.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(ActiveAuthenticationSchemes = "website")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}