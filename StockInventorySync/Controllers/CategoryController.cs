using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using StockInventorySync.DataAccess.Data;
using StockInventorySync.Models;
using StockInventorySync.Utilities;

namespace StockInventorySync.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CategoryController> _logger;
        //[BindProperty(Categories category)]
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Category_Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Model validation failed for Category Creation.");
                return View(category);
            }

            try
            {
                bool isExistingCategory = await _context.Categories.AnyAsync(c => c.Name == category.Name);

                if (!isExistingCategory)
                {
                    category.DateCreated = DateTime.Now;
                    category.CreatedBy = "MIKE";
                    category.Status = StaticDetails.Status_Active;
                    category.DateActivatedDeactivated = DateTime.Now;
                    category.ActivatedDeactivatedBy = "MIKE";


                    await _context.AddAsync(category);
                    await _context.SaveChangesAsync();

                    TempData["success"] = "Category created successfully!";

                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("Name", "Category Name already Exists.");

            }
            catch (Exception)
            {

                throw;

            }

            return View(category);

        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Category_Id,Name,Description")] Category category)
        {
            if (id != category.Category_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingCategory = await _context.Categories
                                                    .AsNoTracking()
                                                    .FirstOrDefaultAsync(c => c.Category_Id == id);

                if (existingCategory == null)
                {
					return NotFound();
				}

                category.DateCreated = existingCategory.DateCreated;
                category.CreatedBy = existingCategory.CreatedBy;
                category.Status = existingCategory.Status;
				category.DateActivatedDeactivated = existingCategory.DateActivatedDeactivated;
				category.ActivatedDeactivatedBy = existingCategory.ActivatedDeactivatedBy; ;

				bool isExistingCategoryName = await _context.Categories.AnyAsync(c => c.Category_Id != id && c.Name == category.Name);

                if (isExistingCategoryName)
                {
                    ModelState.AddModelError("Name", "Category Name already Exists.");
                    return View(category);
                }

                try
                {

                    category.DateUpdated = DateTime.Now;
                    category.UpdatedBy = "MIKE";


                    _context.Categories.Update(category);
                    await _context.SaveChangesAsync();

                    TempData["success"] = "Category updated successfully!";

                }
                catch (DbUpdateConcurrencyException)
                {

                    if (!CategoryExists(category.Category_Id))
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

            return View(category);
        }

        
        public async Task<IActionResult> LockUnlockStatus(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Category_Id == id);
            if (category == null)
            {
                return NotFound();
            }

			category.DateActivatedDeactivated = DateTime.Now;
			category.ActivatedDeactivatedBy = "MIKE";

			if (category.Status == StaticDetails.Status_Active)
            {
                category.Status = StaticDetails.Status_Inactive;
				TempData["success"] = $"Category Status change to {StaticDetails.Status_Inactive.ToUpper()}! You're not allowed to modify this Category.";
			}
            else
            {
                category.Status = StaticDetails.Status_Active;
				TempData["success"] = $"Category Status change to {StaticDetails.Status_Active.ToUpper()}!";
			}

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            

            return RedirectToAction(nameof(Index));
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Category_Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();

            TempData["success"] = "Category deleted successfully!";

            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Category_Id == id);
        }
    }
}
