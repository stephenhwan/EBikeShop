using CleanArchitect.Application.DTOs.BikeShop;
using CleanArchitect.Application.Interfaces;
using CleanArchitect.Common.Contants;
using CleanArchitect.Infrastructure.DbContexts;
using CleanArchitect.MVC.Utils;
using CleanArchitect.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitect.MVC.Controllers
{
    //[Authorize]
    public class BikesController : Controller
    {
        private readonly EBikeShopDbContext _context;
        private readonly IBikeService _serviceBike;

        public BikesController(EBikeShopDbContext context, IBikeService serviceBike)
        {
            _context = context;
            _serviceBike = serviceBike;
        }

        // GET: Bikes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Bikes.ToListAsync());
        }

        // GET: Bikes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var bike = await _context.Bikes
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var bikeVM = await _context.Bikes
                .Where(m => m.Id == id)
                .Select(b => new BikeVM
                {
                    Id = b.Id,
                    Name = b.Name,
                    BrandName = b.BrandName,
                    //Category = b.Category,
                    Description = b.Description,
                    Position = b.Position,
                    Year = b.Year,
                })
                .SingleOrDefaultAsync();
            if (bikeVM == null)
            {
                return NotFound();
            }

            return PartialView(bikeVM);
        }

        // GET: Bikes/Create
        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories
                .OrderByDescending(c => c.Position)
                .Select(c => new
                {
                    Id = c.Id,
                    Name = c.Position + ". " + c.Name
                })
                .ToListAsync();
            ViewBag.CategoryList = new SelectList(categories, "Id", "Name");
            //ViewBag.CategoryList = new SelectList(categories, "Id", "Description");
            return View();
        }

        // POST: Bikes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BikeVM bikeVM)
        {
            if (ModelState.IsValid)
            {
                var file = bikeVM.Image;
                var storedFileName = await MediaUtility.SaveImage(file);

                //var countBikes = await _context.Bikes.CountAsync();
                var bikeDTO = new BikeDTO
                {
                    Name = bikeVM.Name.Trim(),
                    BrandName = bikeVM.BrandName.Trim(),
                    CategoryId = bikeVM.CategoryId,
                    //Category = bikeVM.Category.Trim(),
                    Description = bikeVM.Description?.Trim(),
                    Year = bikeVM.Year,
                    ImagePath = storedFileName,
                    //Position = ++countBikes
                };

                var bike = await _serviceBike.Create(bikeDTO);
                //bike.ImageName = storedFileName;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Create));
            }
            return View(bikeVM);
        }

        // GET: Bikes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bike = await _context.Bikes.FindAsync(id);
            if (bike == null)
            {
                return NotFound();
            }
            var bikeVM = new BikeVM
            {
                Id = bike.Id,
                Name = bike.Name,
                BrandName = bike.BrandName,
                Year = bike.Year,
                ImagePath = bike.ImageName != null
                    ? Path.Combine("~/", AppConstants.ImageFolderPath, bike.ImageName)
                    : Path.Combine("~/", AppConstants.ImageDefault),
                //Category = bike.Category,
                CategoryId = bike.CategoryId,
                Description = bike.Description,
                Position = bike.Position,
            };
            var categories = await _context.Categories
                .OrderByDescending(c => c.Position)
                .Select(c => new
                {
                    Id = c.Id,
                    Name = c.Position + ". " + c.Name
                })
                .ToListAsync();
            ViewBag.CategoryList = new SelectList(categories, "Id", "Name");
            return View(nameof(Create), bikeVM);
        }

        // POST: Bikes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, BikeVM bikeVM)
        {
            if (id != bikeVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(bike);
                    var bike = await _context.Bikes.FindAsync(bikeVM.Id);
                    if (bike != null)
                    {
                        bike.Name = bikeVM.Name.Trim();
                        bike.BrandName = bikeVM.BrandName.Trim();
                        bike.Year = bikeVM.Year;
                        bike.CategoryId = bikeVM.CategoryId;
                        //bike.Category = bikeVM.Category.Trim();
                        bike.Description = bikeVM?.Description;

                        #region Xử lý ảnh
                        var file = bikeVM.Image;
                        var storedFileName = await MediaUtility.SaveImage(file);
                        #endregion
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BikeExists(bikeVM.Id))
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
            //return View(bike);
            return View(nameof(Create), bikeVM);
        }

        // GET: Bikes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bike = await _context.Bikes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bike == null)
            {
                return NotFound();
            }

            return View(bike);
        }

        // POST: Bikes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var bike = await _context.Bikes.FindAsync(id);
            if (bike != null)
            {
                _context.Bikes.Remove(bike);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //public async Task<IActionResult> AutioFixCategoryId()
        //{
        //    bool isOK = false;
        //    string message = "Chưa thực thi";

        //    try
        //    {
        //        var bikes = await _context.Bikes
        //        .Where(b => b.Category222 != null || b.Category222 != "")
        //        .ToListAsync();
        //        //var category = await _context.Categories.ToListAsync();
        //        var countCate = await _context.Categories.CountAsync();
        //        //var currentPosition = countCate;
        //        if (bikes != null && bikes.Count > 0)
        //        {
        //            foreach (var bike in bikes)
        //            {
        //                var cateName = bike.Category222.Trim().ToUpper();
        //                var category = await _context.Categories
        //                    .Where(c => c.Name.Trim().ToUpper() == cateName)
        //                    .FirstOrDefaultAsync();
        //                if (category != null)
        //                {
        //                    bike.CategoryId = category.Id;
        //                }
        //                else
        //                {
        //                    var newCategory = new Category
        //                    {
        //                        Name = bike.Category222.Trim(),
        //                        Position = ++countCate,
        //                    };
        //                    _context.Categories.Add(newCategory);
        //                    bike.CategoryId = newCategory.Id;
        //                }
        //                await _context.SaveChangesAsync();
        //            }
        //            isOK = true;
        //            message = "Chạy thành công";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        message = "Lỗi " + ex.Message;
        //    }


        //    return Json(new { isOK, message });
        //}

        private bool BikeExists(Guid id)
        {
            return _context.Bikes.Any(e => e.Id == id);
        }

        //private async Task<bool> SaveImage(IFormFile file, Bike bike)
        //{
        //    bool isOK = false;
        //    #region Xử lý ảnh
        //    //var file = bikeVM.Image;
        //    if (file != null && file.Length > 0)
        //    {
        //        string[] validImages = { ".jpg", ".jpeg", ".png", ".webp" };
        //        var fileName = file.FileName;
        //        var extension = Path.GetExtension(file.FileName).ToLower();
        //        if (validImages.Contains(extension))
        //        {
        //            //if (file.Length > 5242880) return BadRequest("File không được vượt quá 5MB.");
        //            if (file.Length > 5242880) return isOK;
        //            var storedFileName = Guid.NewGuid().ToString() + extension;
        //            var filePath = Path.Combine("wwwroot", AppConstants.ImageFolderPath, storedFileName);
        //            using (var fileStream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await file.CopyToAsync(fileStream);
        //            }
        //            bike.ImageName = storedFileName;
        //            isOK = true;
        //        }
        //    }
        //    #endregion
        //    return isOK;
        //}
    }
}
