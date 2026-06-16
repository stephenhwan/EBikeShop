using EBikeShop.MVC.Configs;
using EBikeShop.MVC.Data;
using EBikeShop.MVC.Data.Entities;
using EBikeShop.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EBikeShop.MVC.Controllers
{
	[Authorize]
	public class BikesController : Controller
	{
		private readonly EBikeShopDbContext _context;

		public BikesController(EBikeShopDbContext context)
		{
			_context = context;
		}

		// GET: Bikes
		public async Task<IActionResult> Index()
		{
			return View(await _context.Bikes
			.Include(b => b.Category)
			.OrderByDescending(b => b.Position)
			.Select(b => new BikeVM
			{
				Id = b.Id,
				Name = b.Name,
				BrandName = b.BrandName,
				CategoryName = b.Category != null ? b.Category.Name : "",
				Description = b.Description,
				Position = b.Position,
				Year = b.Year,
			})
			.ToListAsync());
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
				.Include(b => b.Category)
				.Where(m => m.Id == id)
				.Select(b => new BikeVM
				{
					Id = b.Id,
					Name = b.Name,
					BrandName = b.BrandName,
					CategoryName = b.Category != null ? b.Category.Name : "",
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
				var countBikes = await _context.Bikes.CountAsync();
				string categoryName = "";
				if (bikeVM.CategoryId.HasValue)
				{
					var category = await _context.Categories.FindAsync(bikeVM.CategoryId.Value);
					categoryName = category?.Name ?? "";
				}

				var bike = new Bike
				{
					Id = Guid.NewGuid(),
					Name = bikeVM.Name.Trim(),
					BrandName = bikeVM.BrandName.Trim(),
					CategoryId = bikeVM.CategoryId,
					CategoryName = categoryName,
					Description = bikeVM.Description?.Trim(),
					Year = bikeVM.Year,
					Position = ++countBikes
				};
				_context.Bikes.Add(bike);

				//	#region Xử lý ảnh
				var file = bikeVM.Image;
				var isOK = await SaveImage(file, bike);
				if (isOK)
				{
					await _context.SaveChangesAsync();
					return RedirectToAction(nameof(Create));
				}
				else
				{
					return BadRequest("File không được vượt quá 5MB.");
				}
			}
			return View(bikeVM);
		}


		// GET: Bikes/Edit/5
		public async Task<IActionResult> Edit(Guid? id)
		{
			if (id == null) return NotFound();

			var bike = await _context.Bikes.FindAsync(id);
			if (bike == null) return NotFound();

			// ✅ Lấy ảnh đầu tiên qua BikeMedia
			//var firstMedia = await _context.BikeMedias
			//	.Where(bm => bm.BikeId == id)
			//	.Select(bm => bm.Media.FilePath)
			//	.FirstOrDefaultAsync();

			var bikeVM = new BikeVM
			{
				Id = bike.Id,
				Name = bike.Name,
				BrandName = bike.BrandName,
				Year = bike.Year,
				CategoryId = bike.CategoryId,
				Description = bike.Description,
				Position = bike.Position,
				ImagePath = bike.ImageName != null
					? Path.Combine("~/", AppConstants.ImageFolderPath, bike.ImageName)
					: Path.Combine("~/", AppConstants.ImageDefault),
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
						var file = bikeVM.Image;
						var isOK = await SaveImage(file, bike);
						if (isOK)
						{
							await _context.SaveChangesAsync();
						}
						else
						{
							return BadRequest("File không được vượt quá 5MB.");
						}
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
				return RedirectToAction(nameof(Index));
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

		private bool BikeExists(Guid id)
		{
			return _context.Bikes.Any(e => e.Id == id);
		}

		private async Task<bool> SaveImage(IFormFile file, Bike bike)
		{
			bool isOK = false;
			#region Xử lý ảnh
			//var file = bikeVM.Image;
			if (file != null && file.Length > 0)
			{
				string[] validImages = { ".jpg", ".jpeg", ".png", ".webp" };
				var fileName = file.FileName;
				var extension = Path.GetExtension(file.FileName).ToLower();
				if (validImages.Contains(extension))
				{
					//if (file.Length > 5242880) return BadRequest("File không được vượt quá 5MB.");
					if (file.Length > 5242880) return isOK;
					var storedFileName = Guid.NewGuid().ToString() + extension;
					var filePath = Path.Combine("wwwroot", AppConstants.ImageFolderPath, storedFileName);
					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						await file.CopyToAsync(fileStream);
					}
					bike.ImageName = storedFileName;
					var media = new Media
					{
						Id = Guid.NewGuid(),
						FileName = file.FileName,
						StoredFileName = storedFileName,
						FilePath = Path.Combine(AppConstants.ImageFolderPath, storedFileName),
						FileType = extension,
						FileSize = file.Length,
					};
					_context.Medias.Add(media);
					var bikeMedia = new BikeMedia
					{
						BikeId = bike.Id,
						MediaId = media.Id,
					};
					_context.BikeMedias.Add(bikeMedia);

					isOK = true;
				}
			}
			#endregion
			return isOK;
		}
	}
}