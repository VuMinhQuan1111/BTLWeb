using BTLWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BTLWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        BtlwebContext db = new BtlwebContext();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        


        public IActionResult IndexPost()
        {
            var listPosts = db.TblPosts.ToList();
            return View(listPosts);
        }

        public async Task<IActionResult> PostDetail(int? id, string searchString)
        {
            /*if (id == null)
            {
                return NotFound();
            }
            if (searchString != null)
            {
                return View("IndexSong", await _context.Songs.Where(s => s.Name.Contains(searchString)).ToListAsync());
            }
            var song = await _context.Songs
                .FirstOrDefaultAsync(m => m.Id == id);
            song.View = song.View + 1;
            _context.Update(song);
            await _context.SaveChangesAsync();
            Favorite userfavor = _context.Favorites.Where(m => m.UserName == HttpContext.Session.GetString("username") && m.Songid == id).FirstOrDefault();
            if (userfavor == null)
            {
                ViewBag.Like = 1;
            }
            else
            {
                ViewBag.Like = 2;
            }
            if (song == null)
            {
                return NotFound();
            }

            ViewBag.Artit = await _context.Artists.Where(m => m.Name == song.Artist).ToListAsync();
            ViewBag.Similar = await _context.Songs.Where(m => m.Style == song.Style).ToListAsync();*/
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
