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

        public AccountController(ILogger<AccountController> logger, BtlwebContext context)
        {
            _logger = logger;
            _context = context;
        }
        
        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Login(TblUser _tblUser)
        {
            BtlwebContext _tblwebContext = new BtlwebContext();
            /*string user = field["username"];
            string pass = field["pass"];*/
            /*TblUser rowuser = _context.TblUsers.Where(m => m.Role == "User" && (m.Name == user)).FirstOrDefault();*/
            /*TblUser rowuser1 = _context.TblUsers.Where(m => m.Status == 1 && m.Role == "User" && (m.Name == user)).FirstOrDefault();*/
            var status = _tblwebContext.TblUsers.Where(m => m.UsersName == _tblUser.UsersName && m.UsersPass == _tblUser.UsersPass).FirstOrDefault();
            if(status == null)
            {
                ViewBag.ancap = "Mật khẩu sai rồi";
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            /*if (rowuser == null)
            {
                ViewBag.ancap = "Tài khoản này không tồn tại";
                *//*loginAttempts++;*//*
            }
            else if (rowuser1 == null)
            {
                ViewBag.ancap = "Tài khoản bị khóa";
            }
            else
            {
                // check tài khoản đúng thì check mật khẩu
                if ((rowuser.Pass) == Encrypt.MD5Hash(pass))
                {
                    HttpContext.Session.SetString("username", user);
                    HttpContext.Session.SetString("id", rowuser.Id.ToString());
                    *//*loginAttempts = 0;*//*
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ancap = "Mật khẩu sai rồi";
                    *//*loginAttempts++;*//*
                }
            }*/
            return View(_tblUser);
        }
        public IActionResult Register()
        {
            return View();
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
