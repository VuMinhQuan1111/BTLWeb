using BTLWeb.Models;
using BTLWeb.Models.Authen;
using BTLWeb.Models.ModelsView;
using BTLWeb.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BTLWeb.Controllers
{
    public class AccountController : Controller
    {
        // GET: AccountController
        private readonly ILogger<AccountController> _logger;
        private readonly BtlwebContext _context;
        BtlwebContext db = new BtlwebContext();

        public AccountController(ILogger<AccountController> logger, BtlwebContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Authentication]
        public IActionResult Index()
        {
            
            if (HttpContext.Session.GetInt32("UsersId") == null)
            {
                return View("Login");
            }
            else
            {
                TblUser? userinfor = _context.TblUsers
                    .Where(m => m.UsersId == HttpContext.Session.GetInt32("UsersId"))
                    .Include(u => u.TblFavorites).ThenInclude(f => f.Post)
                    .FirstOrDefault();
                if (userinfor == null) return RedirectToAction("Index", "Home");
                ViewBag.Soyt = _context.TblFavorites.Where(m => m.UsersId == HttpContext.Session.GetInt32("UsersId")).Count();
                return View(userinfor);
            }
        }

        
        [HttpGet]
        public IActionResult Login()
        {
            if (AuthService.IsAuthenticated(HttpContext))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
            /*if(HttpContext.Session.GetString("UsersName") == null) {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }*/
            
        }

        [HttpPost]
        /*public IActionResult Login(TblUser tblUser)*/
        public IActionResult Login(MV_SignIn data)
        {
            if (ModelState.IsValid)
            {
                if(AuthService.CreateSession(HttpContext, data))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("UsersName", "Tên tài khoản hoặc mật khẩu chưa đúng");
                }
                return View();
            }
            return View();

            /*System.Diagnostics.Debug.WriteLine(tblUser.UsersEmail);
            if (HttpContext.Session.GetString("UsersName") == null)
            {
                var u = db.TblUsers.Where(x => x.UsersName.Equals(tblUser.UsersName) && x.UsersPass.Equals(tblUser.UsersPass)).FirstOrDefault();
                if (u == null)
                {
                    ViewBag.alert = "Tài khoản không tồn tại";
                }
                else if (u != null)
                {
                    HttpContext.Session.SetString("UsersName", u.UsersName.ToString());
                    //System.Diagnostics.Debug.WriteLine(HttpContext.Session.GetString("UsersName"));
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.alert = "Tài khoản hoặc mật khẩu sai rồi";
                }
            }
            System.Diagnostics.Debug.WriteLine("Hello there");
            return View(tblUser);*/
        }

        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(MV_Register data)
        {
            if (ModelState.IsValid)
            {
                if (AuthService.CreateUser(data))
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            return View();
        }
        /*public async Task<IActionResult> Register([Bind("UsersId, UsersName, UsersEmail, UsersPass, UsersRole")] TblUser tblUser)
        {
            var user = new TblUser
            {
                UsersId = tblUser.UsersId,
                UsersName = tblUser.UsersName,
                UsersEmail = tblUser.UsersEmail,
                UsersPass = tblUser.UsersPass,
                UsersRole = "Client"
            };

            if (ModelState.IsValid)
            {
                if (_context.TblUsers.Any(x => x.UsersName == tblUser.UsersName))
                {
                    ViewBag.alert = "Tên đã được sử dụng";
                }
                else if (_context.TblUsers.Any(x => x.UsersEmail == tblUser.UsersEmail))
                {
                    ViewBag.alert = "Email đã tồn tại";
                }
                else
                {
                    _context.TblUsers.AddAsync(user);
                    await _context.SaveChangesAsync();
                    ViewBag.alert = "Thành công";

                }
            }
            return View();
        }*/
            /*string username = fielt["username"];
            string pass = fielt["pass"];
            string email = fielt["email"];
            string repass = fielt["repass"];
            TblUser rowuser = _context.TblUsers.Where(m => m.UsersName == username).FirstOrDefault();
            if (rowuser != null)
            {
                ViewBag.alert("Tài khoản đã tồn tại");
            }
            else if (pass != repass)
            {
                ViewBag.ancap = "Mật khẩu không trùng khớp";
            }
            else
            {
                tblUser.UsersName = username;
                tblUser.UsersEmail = email;
                tblUser.UsersPass = pass;
                tblUser.UsersRole = "User";
                _context.Add(tblUser);
                await _context.SaveChangesAsync();
                ViewBag.ancap = "Đăng ký tài khoản thành công";
            }
            return View();*/
        

        [HttpGet]
        public IActionResult LogOut()
        {
            /*HttpContext.Session.Clear();
            HttpContext.Session.Remove("UsersId");*/
            AuthService.Logout(HttpContext);
            return RedirectToAction("Login", "Account");
            /*return Redirect(Request.Headers["Referer"].ToString());*/
        }


        [HttpPost]
        public IActionResult Favorite(int postId)
        {
            //var usersId = HttpContext.Session.GetInt32("UsersId");
            //if(usersId == null)
            //{
            //    return View("Login", "Account");
            //}
            //else
            var usersId = HttpContext.Session.GetInt32("UsersId"); // sao k lấy id ở đây luôn đi wtf
            if (ModelState.IsValid) 
            { 
                TblFavorite userFav = _context.TblFavorites.FirstOrDefault(m => m.UsersId == usersId && m.PostId == postId);
                if (userFav == null)
                {
                    TblFavorite tblFavorite = new TblFavorite
                    {
                        UsersId = (int)usersId, // lỗi đây này, 
                        PostId = postId
                    };
                    _context.TblFavorites.Add(tblFavorite);
                    _context.SaveChanges();
                    return Json(new { isFav = true });

                }
                else
                {
                    _context.TblFavorites.Remove(userFav);
                    _context.SaveChanges();
                    return Json(new { isFav = false });

                }
            }
            //return Json(new { isLiked = false });
            return Json(new { isFav = false });
        }

        [HttpGet]
        public IActionResult GetFavCount(int id)
        {
            int favCount = _context.TblFavorites.Where(m => m.PostId == id).Count();
                return Json(new { favCount = favCount });
        }

        public IActionResult LikePostList()
        {
            
            if (HttpContext.Session.GetInt32("UsersId") == null)
            {
                return View("Login");
            }
            else
            {
                var posts = (from p in _context.TblPosts.Include(t => t.Category).Include(t => t.Users)
                             join o in _context.TblFavorites
                             on p.PostId equals o.PostId
                             where o.UsersId == HttpContext.Session.GetInt32("UsersId")
                             select p).ToPagedList();

                return View(posts);
            }
        }

        public IActionResult ChangePassword()
        {
            if (HttpContext.Session.GetInt32("UsersId") == null)
            {
                return View("Login");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(MV_Changepass changepass)
        {
            var userinfo = _context.TblUsers.Where(m => m.UsersId == HttpContext.Session.GetInt32("UsersId")).FirstOrDefault();
            if (ModelState.IsValid)
            {

            }
            return View(changepass);
        }
        // GET: AccountController/Details/5
        /*public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccountController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccountController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccountController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }*/
    }
}
