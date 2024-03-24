using BTLWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BTLWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

    }
}
