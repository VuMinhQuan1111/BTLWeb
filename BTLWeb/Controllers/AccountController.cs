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
        
        public IActionResult Index()
        {
            var listUsers = db.TblUsers.ToList();
            return View(listUsers);
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
            if (HttpContext.Session.GetString("UsersName") == null)
            {
                var u = db.TblUsers.Where(x => x.UsersName.Equals(tblUser.UsersName) && x.UsersPass.Equals(tblUser.UsersPass)).FirstOrDefault();
                if (u != null)
                {
                    HttpContext.Session.SetString("UsersName", u.UsersName.ToString());
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(tblUser);
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
