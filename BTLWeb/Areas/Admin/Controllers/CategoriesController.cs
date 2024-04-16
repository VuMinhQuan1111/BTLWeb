using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BTLWeb.Models;
using BTLWeb.Models.ModelsView;
using BTLWeb.Models.Dto;

namespace BTLWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly BtlwebContext _context;

        public CategoriesController(BtlwebContext context)
        {
            _context = context;
        }

        // GET: Admin/Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.TblCategories.ToListAsync());
        }

        // GET: Admin/Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblCategory = await _context.TblCategories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (tblCategory == null)
            {
                return NotFound();
            }

            return View(tblCategory);
        }

        // GET: Admin/Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryName,CategoryDescription")] MV_Categories mv_categories)
        {
            if (ModelState.IsValid)
            {
                TblCategory tblCategory = new TblCategory()
                {
                    
                    CategoryName = mv_categories.CategoryName,
                    CategoryDescription = mv_categories.CategoryDescription,
                    
                };
                _context.Add(tblCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mv_categories);
        }

        // GET: Admin/Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblCategory = await _context.TblCategories.FindAsync(id);
            if (tblCategory == null)
            {
                return NotFound();
            }
            return View(tblCategory);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,CategoryDescription")] TblCategory tblCategory)
        {
            if (id != tblCategory.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblCategoryExists(tblCategory.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tblCategory);
        }

        // GET: Admin/Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblCategory = await _context.TblCategories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (tblCategory == null)
            {
                return NotFound();
            }

            return View(tblCategory);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblCategory = await _context.TblCategories.FindAsync(id);
            if (tblCategory != null)
            {
                _context.TblCategories.Remove(tblCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblCategoryExists(int id)
        {
            return _context.TblCategories.Any(e => e.CategoryId == id);
        }
    }
}
