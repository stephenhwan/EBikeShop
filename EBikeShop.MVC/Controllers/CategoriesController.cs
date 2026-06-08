using EBikeShop.MVC.Data;
using EBikeShop.MVC.Data.Entities;
using EBikeShop.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EBikeShop.MVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly EBikeShopDbContext _context;

        public CategoriesController(EBikeShopDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(Guid? idCategory)
        {
            if (idCategory == null)
            {
                return NotFound();
            }

            var categoryVM = await _context.Categories
                .Where(c => c.Id == idCategory)
                .Select(c => new CategoryVM
                {
                    Id = c.Id,
                    Description = c.Description,
                    Name = c.Name,
                    Position = c.Position,
                })
                .SingleOrDefaultAsync();
            if (categoryVM == null)
            {
                return NotFound();
            }

            return PartialView(categoryVM);
        }

        // GET: Categories/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryVM categoryVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //category.Id = Guid.NewGuid();
                    var countCategory = await _context.Categories.CountAsync();
                    //category.Position = ++countCategory;
                    //_context.Add(category);
                    var newCategory = new Category
                    {
                        //Id = Guid.NewGuid(),
                        Name = categoryVM.Name.Trim(),
                        Description = categoryVM.Description?.Trim(),
                        Position = ++countCategory
                    };
                    _context.Categories.Add(newCategory);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Create));
                }
                catch (Exception)
                {

                }
            }
            return View(categoryVM);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
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
            var categoryVM = new CategoryVM
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Position = category.Position
            };
            return View(nameof(Create), categoryVM);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CategoryVM categoryVM)
        {
            if (id != categoryVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(category);
                    var oldCate = await _context.Categories.FindAsync(categoryVM.Id);
                    if (oldCate != null)
                    {
                        oldCate.Name = categoryVM.Name.Trim();
                        oldCate.Description = categoryVM.Description?.Trim();
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(categoryVM.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Create));
            }
            return View(nameof(Create), categoryVM);
            //return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(Guid? idCategory)
        {
            if (idCategory == null)
            {
                return NotFound();
            }

            var categoryVM = await _context.Categories
                .Select(c => new CategoryVM
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Position = c.Position
                })
                .SingleOrDefaultAsync(m => m.Id == idCategory);
            if (categoryVM == null)
            {
                return NotFound();
            }

            return PartialView(categoryVM);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid idCategory)
        {
            var category = await _context.Categories.FindAsync(idCategory);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(Guid id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
