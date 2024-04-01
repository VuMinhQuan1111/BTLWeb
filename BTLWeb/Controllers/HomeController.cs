using BTLWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using BTLWeb.Models.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BTLWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        BtlwebContext db = new BtlwebContext();
        private readonly BtlwebContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment, BtlwebContext context)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        


        public async Task<IActionResult> IndexPost()
        {
            var btlwebContext = _context.TblPosts.Include(t => t.Category).Include(t => t.Users).OrderByDescending(p => p.PostId);
            
            return View(await btlwebContext.ToListAsync());
        }

        public async Task<IActionResult> PostDetail(int? id, string searchString)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblPost = await _context.TblPosts
                .Include(t => t.Category)
                .Include(t => t.Users)
                .FirstOrDefaultAsync(m => m.PostId == id);
            /*TblFavorite userfavor = _context.TblFavorites.Where(m => m.UsersId == HttpContext.Session.GetString("username") && m.PostId == id).FirstOrDefault();
            if (userfavor == null)
            {
                ViewBag.Like = 1;
            }
            else
            {
                ViewBag.Like = 2;
            }*/
            if (tblPost == null)
            {
                return NotFound();
            }

            return View(tblPost);
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
            /*return View();*/
        }

        public IActionResult CreatePost()
        {
            ViewData["CategoryId"] = new SelectList(db.TblCategories, "CategoryId", "CategoryName");
            ViewData["UsersId"] = new SelectList(_context.TblUsers, "UsersId", "UsersName");
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreatePost([Bind("PostId,UsersId,CategoryId,PostTitle,PostContent,PostImg,PostAuthor,PostCreateAt")] TblPostDto tblPostDto)
        {
            try
            {
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssff");
                newFileName += Path.GetExtension(tblPostDto.PostImg!.FileName);

                string imageFullPath = _webHostEnvironment.WebRootPath + "/PostImg/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    tblPostDto.PostImg.CopyTo(stream);
                }

                TblPost tblPost = new TblPost()
                {
                    UsersId = tblPostDto.UsersId,
                    CategoryId = tblPostDto.CategoryId,
                    PostTitle = tblPostDto.PostTitle,
                    PostContent = tblPostDto.PostContent,
                    PostImg = newFileName,
                    PostAuthor = tblPostDto.PostAuthor,
                    PostCreateAt = DateTime.Now
                };
                _context.Add(tblPost);
                await _context.SaveChangesAsync();
                return RedirectToAction("IndexPost", "Home");
            }
            catch (DbUpdateException ex)
            {

                string errorMessage = ex.InnerException?.Message ?? ex.Message;
                ViewBag.ErrorMessage = errorMessage;
            }
            

            ViewData["CategoryId"] = new SelectList(_context.TblCategories, "CategoryId", "CategoryName", tblPostDto.CategoryId);
            ViewData["UsersId"] = new SelectList(_context.TblUsers, "UsersId", "UsersName", tblPostDto.UsersId);
            return View(tblPostDto);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
