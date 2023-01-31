using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NewSystem.Controllers
{
    public class Home2Controller : Controller
    {
        [Authorize(Roles = "2")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
