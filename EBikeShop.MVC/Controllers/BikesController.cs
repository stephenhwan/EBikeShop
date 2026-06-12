using EBikeShop.MVC.Configs;
using EBikeShop.MVC.Data;
using EBikeShop.MVC.Data.Entities;
using EBikeShop.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EBikeShop.MVC.Controllers
{
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

				#region Xử lý ảnh
				var file = bikeVM.Image;
				if (file != null && file.Length > 0)
				{
					string[] validImages = { ".jpg", ".jpeg", ".png" };
					var extension = Path.GetExtension(file.FileName).ToLower();

					if (!validImages.Contains(extension))
					{
						ModelState.AddModelError("Image", "Chỉ chấp nhận .jpg, .jpeg, .png");
						return View(bikeVM); // ✅ return sớm nếu sai định dạng
					}

					if (file.Length > 5242880)
						return BadRequest("File không được vượt quá 5MB.");
					//lưu đường dẫn trên ổ đĩa 
					var storedFileName = Guid.NewGuid().ToString() + extension;
					var filePath = Path.Combine("wwwroot", AppConstants.ImageFolderPath, storedFileName);

					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						await file.CopyToAsync(fileStream);
					}

					//lưu vào db
					var webPath = $"/{AppConstants.ImageFolderPath.Replace("\\", "/")}/{storedFileName}";
				
				var media = new Media
				{
					Id = Guid.NewGuid(),
					FileName = file.FileName,
					StoredFileName = storedFileName,
					FilePath = webPath,
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
			}
				#endregion

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
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
			var firstMedia = await _context.BikeMedias
				.Where(bm => bm.BikeId == id)
				.Select(bm => bm.Media.FilePath)
				.FirstOrDefaultAsync();

			var bikeVM = new BikeVM
			{
				Id = bike.Id,
				Name = bike.Name,
				BrandName = bike.BrandName,
				Year = bike.Year,
				CategoryId = bike.CategoryId,
				Description = bike.Description,
				Position = bike.Position,
				ImagePath = firstMedia  // ✅ Lấy từ DB
			};

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
					}
					await _context.SaveChangesAsync();
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

		public async Task<IActionResult> AutioFixCategoryId()
		{
			bool isOK = false;
			string message = "Chưa thực thi";

			try
			{
				var bikes = await _context.Bikes
				.Where(b => b.CategoryName != null && b.CategoryName != "")
				.ToListAsync();
				//var category = await _context.Categories.ToListAsync();
				var countCate = await _context.Categories.CountAsync();
				//var currentPosition = countCate;
				if (bikes != null && bikes.Count > 0)
				{
					foreach (var bike in bikes)
					{
						var cateName = bike.CategoryName.Trim().ToUpper();
						var category = await _context.Categories
							.Where(c => c.Name.Trim().ToUpper() == cateName)
							.FirstOrDefaultAsync();
						if (category != null)
						{
							bike.CategoryId = category.Id;
						}
						else
						{
							var newCategory = new Category
							{
								Name = bike.CategoryName.Trim(),
								Position = ++countCate,
							};
							_context.Categories.Add(newCategory);
							bike.CategoryId = newCategory.Id;
						}
						await _context.SaveChangesAsync();
					}
					isOK = true;
					message = "Chạy thành công";
				}
			}
			catch (Exception ex)
			{
				message = "Lỗi " + ex.Message;
			}


			return Json(new { isOK, message });
		}

		private bool BikeExists(Guid id)
		{
			return _context.Bikes.Any(e => e.Id == id);
		}
	}
}