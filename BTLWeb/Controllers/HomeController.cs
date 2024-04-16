using BTLWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using BTLWeb.Models.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using BTLWeb.Models.Authen;
using BTLWeb.Models.RequestModels;
using X.PagedList;
using Azure;
using BTLWeb.Areas.Admin.Controllers;

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


        
        /*public async Task<IActionResult> IndexPost(string searchString, int? page)*/
        public IActionResult IndexPost(string searchString, int? page, string searchByCategory, string cate)
        {

            int pageSize = 3;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            IQueryable<TblPost> btlwebContext = _context.TblPosts
                .Include(t => t.Category)
                .Include(t => t.Users);
            PagedList<TblPost> lst = new PagedList<TblPost>(btlwebContext, pageNumber, pageSize);
            if (Request.Query["sort"].Count() > 0)
            {
                string? sort = Request.Query["sort"];
                if(sort != null)
                {
                    switch (sort)
                    {
                        case "az":
                            btlwebContext = btlwebContext.OrderBy(p => p.PostTitle).AsQueryable();
                            break;
                        case "za":
                            btlwebContext = btlwebContext.OrderByDescending(p => p.PostTitle).AsQueryable();
                            break;
                        case "latest":
                            btlwebContext = btlwebContext.OrderByDescending(p => p.PostCreateAt).AsQueryable();
                            break;
                        case "oldest":
                            btlwebContext = btlwebContext.OrderBy(p => p.PostCreateAt).AsQueryable();
                            break;
                        default:
                            btlwebContext = btlwebContext.OrderByDescending(p => p.PostId).AsQueryable();
                            break;
                    }
                } else {
                    btlwebContext = btlwebContext.OrderByDescending(p => p.PostId).AsQueryable();
                }
            }
            else
            {
                btlwebContext = btlwebContext.OrderByDescending(p => p.PostId).AsQueryable();
            }

            //var lstpost = (from c in _context.TblCategories
            //               join p in _context.TblPosts.Include(c => c.Category)
            //               on c.CategoryId equals p.CategoryId
            //               select p).ToPagedList();

            if (searchString != null)
            {
                /*return View(await btlwebContext.Where(s => s.PostTitle.Contains(searchString)).ToListAsync());*/
                return View(btlwebContext.Where(s => s.PostTitle.Contains(searchString)).ToPagedList());
                /*return View(lst.Where(s => s.PostTitle.Contains(searchString)).ToList());*/
            } else if (searchByCategory != null)
            {
                return View(btlwebContext.Where(s => s.Category.CategoryName.Contains(searchByCategory)).ToPagedList());
            } else if (cate != null)
            {
                return View(btlwebContext.Where(s => s.Category.CategoryName == cate).ToPagedList());
            }


            //var categories = _context.TblCategories.ToList();
            //categories.Insert(0, new TblCategory { CategoryId = 0, CategoryName = "Chọn danh mục" });
            //ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryName");


            /*else
            {
                return View(lst);
                *//*return View(await btlwebContext.ToListAsync());
                return View(await _context.TblPosts.ToListAsync());*//*
            }*/
            return View(lst);

        }

        

        public async Task<IActionResult> PostDetail(int? id, string searchString)
        {
            //ViewBag.FavCount = _context.TblFavorites.Where(m => m.UsersId == HttpContext.Session.GetInt32("UsersId")).Count();
            
            ViewBag.FavCount = _context.TblFavorites.Where(m => m.PostId == id).Count();
            if (id == null)
            {
                return NotFound();
            }

            var tblPost = await _context.TblPosts
                .Include(t => t.Category)
                .Include(t => t.Users)
                .Include(t => t.TblComments).ThenInclude(c => c.Users)
                .Include(t => t.TblFavorites)
                .FirstOrDefaultAsync(m => m.PostId == id);
            ViewData["TblPost"] = tblPost;
            TblFavorite userfavor = _context.TblFavorites.Where(m => m.Users.UsersId == HttpContext.Session.GetInt32("UsersId") && m.PostId == id).FirstOrDefault();
            if (userfavor == null)
            {
                ViewBag.Fav = 1;
            }
           
            if (tblPost == null)
            {
                return NotFound();
            }

            return View(tblPost);
            
            /*return View();*/
        }

        [Authentication]
        public IActionResult CreatePost()
        {
            ViewData["CategoryId"] = new SelectList(db.TblCategories, "CategoryId", "CategoryName");
            //ViewData["UsersId"] = new SelectList(_context.TblUsers, "UsersId", "UsersName");
            return View();
        }


        
        [HttpPost]
        public async Task<IActionResult> CreatePost([Bind("PostId,UsersId,CategoryId,PostTitle,PostContent,PostImg,PostCreateAt")] TblPostDto tblPostDto)
        {
            try
            {
                if (HttpContext.Session.GetInt32("UsersId") == null)
                {
                    return View("Login");
                }
                else
                {
                    if (tblPostDto.PostImg == null)
                    {
                        ModelState.AddModelError("PostImg", "Hãy thêm ảnh bài viết");
                    }
                    if (!ModelState.IsValid)
                    {
                        ViewData["CategoryId"] = new SelectList(_context.TblCategories, "CategoryId", "CategoryName", tblPostDto.CategoryId);
                        return View(tblPostDto);
                    }
                    string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssff");
                newFileName += Path.GetExtension(tblPostDto.PostImg!.FileName);

                string imageFullPath = _webHostEnvironment.WebRootPath + "/PostImg/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    tblPostDto.PostImg.CopyTo(stream);
                }

                TblPost tblPost = new TblPost()
                {
                    UsersId = HttpContext.Session.GetInt32("UsersId") ?? 0,
                    CategoryId = tblPostDto.CategoryId,
                    PostTitle = tblPostDto.PostTitle,
                    PostContent = tblPostDto.PostContent,
                    PostImg = newFileName,
                    //PostAuthor = tblPostDto.PostAuthor,
                    PostCreateAt = DateTime.Now
                };
                ViewData["UsersId"] = HttpContext.Session.GetInt32("UsersId") ?? 0;
                _context.Add(tblPost);
                await _context.SaveChangesAsync();
                return RedirectToAction("IndexPost", "Home");
                }
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

        public IActionResult EditPost(int id)
        {
            
            //ViewData["UsersId"] = new SelectList(_context.TblUsers, "UsersId", "UsersName");

            var tblPost = _context.TblPosts.Find(id);

            if(tblPost == null)
            {
                return RedirectToAction("Index", "Posts");
            }

            if (tblPost.UsersId != HttpContext.Session.GetInt32("UsersId"))
            {
                return Unauthorized();
            }

            var tblPostDto = new TblPostDto()
            {
                CategoryId = tblPost.CategoryId,
                PostTitle = tblPost.PostTitle,
                PostContent = tblPost.PostContent,

                
            };
            ViewData["PostImg"] = tblPost.PostImg;
            ViewData["PostCreateAt"] = tblPost.PostCreateAt.ToString("MM/dd/yyyy");
            ViewData["CategoryId"] = new SelectList(db.TblCategories, "CategoryId", "CategoryName");
            return View(tblPostDto);
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(int id, [Bind("UsersId,CategoryId,PostTitle,PostContent,PostImg")] TblPostDto tblPostDto)
        {
            var tblPost = _context.TblPosts.Find(id);
            if (tblPost == null)
            {
                return RedirectToAction("Index", "IndexPost");
            }
            if(tblPost.UsersId != HttpContext.Session.GetInt32("UsersId"))
            {
                return Unauthorized();
            }  
            //if (!ModelState.IsValid)
            //{
            //    ViewData["PostImg"] = tblPost.PostImg;
            //    //ViewData["PostCreateAt"] = tblPost.PostCreateAt.ToString("MM/dd/yyyy");
            //    ViewData["CategoryId"] = new SelectList(db.TblCategories, "CategoryId", "CategoryName");
            //    return View(tblPostDto);
            //}


            //Update Image file if have image file
            string newFileName = tblPost.PostImg;
            if (tblPostDto.PostImg != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssff");
                newFileName += Path.GetExtension(tblPostDto.PostImg!.FileName);

                string imageFullPath = _webHostEnvironment.WebRootPath + "/PostImg/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    tblPostDto.PostImg.CopyTo(stream);
                }

                //Delete Old Image
                string oldImageFullPath = _webHostEnvironment.WebRootPath + "/PostImg/" + tblPost.PostImg;
                System.IO.File.Delete(oldImageFullPath);
            }

            //Update Database
            tblPost.CategoryId = tblPostDto.CategoryId;
            tblPost.PostTitle = tblPostDto.PostTitle;
            tblPost.PostContent = tblPostDto.PostContent;
            tblPost.PostImg = newFileName;
            //tblPost.PostAuthor = tblPostDto.PostAuthor;

            ViewData["UserId"] = tblPost.UsersId;
            ViewData["PostImg"] = tblPost.PostImg;
            ViewData["PostCreateAt"] = tblPost.PostCreateAt.ToString("MM/dd/yyyy");
            ViewData["CategoryId"] = new SelectList(_context.TblCategories, "CategoryId", "CategoryName", tblPost.CategoryId);
            //ViewData["UsersId"] = new SelectList(_context.TblUsers, "UsersId", "UsersName", tblPost.UsersId);
            await _context.SaveChangesAsync();
            return RedirectToAction("IndexPost", "Home");
        }

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var tblPost = _context.TblPosts.Find(id);

            if (tblPost == null)
            {
                return RedirectToAction("IndexPost", "Home");
            }

            if (tblPost.UsersId != HttpContext.Session.GetInt32("UsersId"))
            {
                return Unauthorized();
            }

            string ImageFullPath = _webHostEnvironment.WebRootPath + "/PostImg/" + tblPost.PostImg;
            System.IO.File.Delete(ImageFullPath);

            _context.TblPosts.Remove(tblPost);
            _context.SaveChanges(true);

            return RedirectToAction("IndexPost", "Home");
            
        }


        [Authentication]
        [HttpPost]
        public JsonResult AddComments([FromBody] AddComments comments)
        {
            if (HttpContext.Session.GetInt32("UsersId") == null)
            {
                return Json(new { success = false });
            }
            BtlwebContext db = new BtlwebContext();
            TblComment comment1 = new TblComment()
            {
                CommentText = comments.comments,
                CreateAt = DateTime.Now,
                PostId = comments.id,
                UsersId = HttpContext.Session.GetInt32("UsersId") ?? 0, 
            };
           
            /*TblUser? user = db.TblUsers.FirstOrDefault();*/
            db.TblComments.Add(comment1);
            db.SaveChanges();
            Console.WriteLine("Add success");
            return Json(new { success = true });
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
