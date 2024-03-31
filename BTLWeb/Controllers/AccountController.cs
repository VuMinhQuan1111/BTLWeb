using BTLWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UsersName") == null)
            {
                return View("Login");
            }
            else
            {
                ViewBag.userinfor = await _context.TblUsers.Where(m => m.UsersName == HttpContext.Session.GetString("UsersName")).ToListAsync();
                /*ViewBag.Soyt = _context.Favorites.Where(m => m.UserName == HttpContext.Session.GetString("username")).Count();*/
                return View();
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            if(HttpContext.Session.GetString("UsersName") == null) {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        [HttpPost]
        public IActionResult Login(TblUser tblUser)
        {
            
            System.Diagnostics.Debug.WriteLine(tblUser.UsersEmail);
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
                    System.Diagnostics.Debug.WriteLine(HttpContext.Session.GetString("UsersName"));
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.alert = "Tài khoản hoặc mật khẩu sai rồi";
                }
            }
            System.Diagnostics.Debug.WriteLine("Hello there");
            return View(tblUser);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([Bind("UsersId, UsersName, UsersEmail, UsersPass, UsersRole")] TblUser tblUser)
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
                if (_context.TblUsers.Any(x=>x.UsersName == tblUser.UsersName)){
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
        }

        [HttpGet]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UsersName");
            return RedirectToAction("Login", "Account");
            /*return Redirect(Request.Headers["Referer"].ToString());*/
        }

        

        // GET: AccountController/Details/5
        public ActionResult Details(int id)
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
        }
    }
}
