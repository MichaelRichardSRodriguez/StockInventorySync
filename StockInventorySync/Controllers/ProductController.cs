using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockInventorySync.DataAccess.Data;
using StockInventorySync.Models;
using StockInventorySync.Utilities;

namespace StockInventorySync.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Products.Include(p => p.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Product_Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewData["Category_Id"] = new SelectList(_context.Categories.Where(c => c.Status == StaticDetails.Status_Active), "Category_Id", "Name");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Product_Id,Name,Description,Category_Id")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.DateCreated = DateTime.Now;
                product.CreatedBy = "MIKE";
                product.Status = StaticDetails.Status_Active;
				product.DateActivatedDeactivated = DateTime.Now;
				product.ActivatedDeactivatedBy = "MIKE";

				_context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Category_Id"] = new SelectList(_context.Categories, "Category_Id", "Name", product.Category_Id);
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["Category_Id"] = new SelectList(_context.Categories, "Category_Id", "Name", product.Category_Id);
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Product_Id,Name,Description,Category_Id")] Product product)
        {
            if (id != product.Product_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = _context.Products.AsNoTracking().FirstOrDefault(p => p.Product_Id == id);

                    if (existingProduct != null)
                    {
                        product.DateCreated = existingProduct.DateCreated;
                        product.CreatedBy = existingProduct.CreatedBy;
                        product.Status = existingProduct.Status;
                        product.DateUpdated = DateTime.Now;
                        product.DateActivatedDeactivated = existingProduct.DateActivatedDeactivated;
                        product.ActivatedDeactivatedBy = existingProduct.ActivatedDeactivatedBy;
                        product.UpdatedBy = "MIKE";
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Product_Id))
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
            ViewData["Category_Id"] = new SelectList(_context.Categories, "Category_Id", "Name", product.Category_Id);
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Product_Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Product_Id == id);
        }

		public async Task<IActionResult> LockUnlockStatus(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = await _context.Products
				.FirstOrDefaultAsync(m => m.Product_Id == id);
			if (product == null)
			{
				return NotFound();
			}

			product.DateActivatedDeactivated = DateTime.Now;
			product.ActivatedDeactivatedBy = "MIKE";

			if (product.Status == StaticDetails.Status_Active)
			{
				product.Status = StaticDetails.Status_Inactive;
				TempData["success"] = $"Product Status change to {StaticDetails.Status_Inactive.ToUpper()}! You're not allowed to modify this Product.";
			}
			else
			{
				product.Status = StaticDetails.Status_Active;
				TempData["success"] = $"Product Status change to {StaticDetails.Status_Active.ToUpper()}!";
			}

			_context.Products.Update(product);
			await _context.SaveChangesAsync();



			return RedirectToAction(nameof(Index));
		}
	}
}
