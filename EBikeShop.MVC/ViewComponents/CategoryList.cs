using EBikeShop.MVC.Data;
using EBikeShop.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EBikeShop.MVC.ViewComponents
{
	public class CategoryList : ViewComponent
	{
		private readonly EBikeShopDbContext _context;
		public CategoryList(EBikeShopDbContext dbContext)
		{
			_context = dbContext;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			//var categoryList = await _context.Categories.ToListAsync();
			//var categoryVMList = new List<CategoryVM>();
			//if (categoryList != null)
			//{
			//    foreach (var item in categoryList)
			//    {
			//        var cateVM = new CategoryVM
			//        {
			//            Id = item.Id,
			//            Name = item.Name,
			//            Description = item.Description,
			//            Position = item.Position,
			//        };
			//        categoryVMList.Add(cateVM);
			//    }
			//}
			var categoryVMList = await _context.Categories
				.OrderByDescending(c => c.Position)
				.Select(c => new CategoryVM
				{
					Id = c.Id,
					Name = c.Name,
					Description = c.Description,
					Position = c.Position,
				})
				.ToListAsync();
			return View(categoryVMList);
		}
	}
}