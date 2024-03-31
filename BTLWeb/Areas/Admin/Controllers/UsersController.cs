using BTLWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTLWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly BtlwebContext _context;

        public UsersController(BtlwebContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var listUser = _context.TblUsers.ToList();
            return View(listUser);
        }
    }
}
