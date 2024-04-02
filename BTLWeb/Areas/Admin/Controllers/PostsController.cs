using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BTLWeb.Models;
using Microsoft.Extensions.Hosting;
using BTLWeb.Models.Dto;
using Microsoft.AspNet.SignalR.Hosting;

namespace BTLWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostsController : Controller
    {
        private readonly BtlwebContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PostsController(BtlwebContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/Posts
        public async Task<IActionResult> Index()
        {
            var btlwebContext = _context.TblPosts.Include(t => t.Category).Include(t => t.Users);
            /*var category = _context.TblCategories.ToList();*/
            return View(await btlwebContext.ToListAsync());
        }

        // GET: Admin/Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblPost = await _context.TblPosts
                .Include(t => t.Category)
                .Include(t => t.Users)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (tblPost == null)
            {
                return NotFound();
            }

            return View(tblPost);
        }

        // GET: Admin/Posts/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.TblCategories, "CategoryId", "CategoryName");
            ViewData["UsersId"] = new SelectList(_context.TblUsers, "UsersId", "UsersName");
            return View();
        }

        // POST: Admin/Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,UsersId,CategoryId,PostTitle,PostContent,PostImg,PostAuthor,PostCreateAt")] TblPostDto tblPostDto, IFormFile imageFile)
        {
            try
            {
                /*if (imageFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "PostImg");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    await imageFile.CopyToAsync(new FileStream(filePath, FileMode.Create));
                    tblPost.PostImg = "/PostImg/" + uniqueFileName;
                }*/
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssff");
                newFileName += Path.GetExtension(tblPostDto.PostImg!.FileName);

                string imageFullPath = _webHostEnvironment.WebRootPath + "/PostImg/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath)){
                    tblPostDto.PostImg.CopyTo(stream);
                }
                    /*if (ModelState.IsValid)
                    {*/

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
                    return RedirectToAction(nameof(Index));
                /*}*/
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

        // GET: Admin/Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblPost = await _context.TblPosts.FindAsync(id);
            if (tblPost == null)
            {
                return NotFound();
            }

            var tblPostDto = new TblPostDto()
            {
                
                PostTitle = tblPost.PostTitle,
                PostContent = tblPost.PostContent,
                PostAuthor = tblPost.PostAuthor,
                
            };

            ViewData["UserId"] = tblPost.UsersId;
            ViewData["PostImg"] = tblPost.PostImg;
            ViewData["PostCreateAt"] = tblPost.PostCreateAt.ToString("MM/dd/yyyy");

            ViewData["CategoryId"] = new SelectList(_context.TblCategories, "CategoryId", "CategoryName", tblPostDto.CategoryId);
            ViewData["UsersId"] = new SelectList(_context.TblUsers, "UsersId", "UsersName", tblPostDto.UsersId);
            return View(tblPostDto);
        }

        // POST: Admin/Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,UsersId,CategoryId,PostTitle,PostContent,PostImg,PostAuthor,PostCreateAt")] TblPostDto tblPostDto)
        {
            var tblPost = _context.TblPosts.Find(id);
            if( tblPost == null)
            {
                return RedirectToAction("Index", "Posts");
            }

            /*if (!ModelState.IsValid)
            {
                ViewData["UserId"] = tblPost.UsersId;
                ViewData["PostImg"] = tblPost.PostImg;
                ViewData["PostCreateAt"] = tblPost.PostCreateAt.ToString("MM/dd/yyyy");

                return View(tblPostDto);
            }*/
            /*if (id != tblPost.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblPostExists(tblPost.PostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }*/

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

                string oldImageFullPath = _webHostEnvironment.WebRootPath + "/PostImg/" + tblPost.PostImg;
                System.IO.File.Delete(oldImageFullPath);
            }

            //Update Database
            tblPost.CategoryId = tblPostDto.CategoryId;
            tblPost.PostTitle = tblPostDto.PostTitle;
            tblPost.PostContent = tblPostDto.PostContent;
            tblPost.PostImg = newFileName;
            tblPost.PostAuthor = tblPostDto.PostAuthor;

            ViewData["UserId"] = tblPost.UsersId;
            ViewData["PostImg"] = tblPost.PostImg;
            ViewData["PostCreateAt"] = tblPost.PostCreateAt.ToString("MM/dd/yyyy");
            ViewData["CategoryId"] = new SelectList(_context.TblCategories, "CategoryId", "CategoryName", tblPost.CategoryId);
            ViewData["UsersId"] = new SelectList(_context.TblUsers, "UsersId", "UsersName", tblPost.UsersId);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Posts");
        }

        // GET: Admin/Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblPost = await _context.TblPosts
                .Include(t => t.Category)
                .Include(t => t.Users)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (tblPost == null)
            {
                return NotFound();
            }

            return View(tblPost);
        }

        // POST: Admin/Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblPost = _context.TblPosts.Find(id);
            if(tblPost == null)
            {
                return RedirectToAction("Index", "Posts");
            }

            string ImageFullPath = _webHostEnvironment.WebRootPath + "/PostImg/" + tblPost.PostImg;
            System.IO.File.Delete(ImageFullPath);

            _context.TblPosts.Remove(tblPost);
            _context.SaveChanges(true);

            return RedirectToAction("Index", "Posts");
            /*var tblPost = await _context.TblPosts.FindAsync(id);
            if (tblPost != null)
            {
                _context.TblPosts.Remove(tblPost);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));*/
        }

        private bool TblPostExists(int id)
        {
            return _context.TblPosts.Any(e => e.PostId == id);
        }
    }
}
