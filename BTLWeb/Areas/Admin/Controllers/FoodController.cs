using BTLWeb.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BTLWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FoodController : Controller
    {
        private readonly BtlwebContext _context;
        public FoodController(BtlwebContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var btlwebContext = _context.TblFoods.Include(t => t.Category);
            return View(btlwebContext.ToList());
        }

        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.TblCategories, "CategoryId", "CategoryName");
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Create()
        //{
        //    return View();
        //}
    }
}
