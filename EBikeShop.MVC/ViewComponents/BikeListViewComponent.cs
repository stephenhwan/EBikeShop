using EBikeShop.MVC.Data;
using EBikeShop.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EBikeShop.MVC.ViewComponents
{
    public class BikeListViewComponent: ViewComponent
    {
        private readonly EBikeShopDbContext _dbContext;
        public BikeListViewComponent(EBikeShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var bikeVMs = await _dbContext.Bikes
                .OrderByDescending(b => b.Position)
                .Select(b => new BikeVM
                {
                    Id = b.Id,
                    Name = b.Name,
                    BrandName = b.BrandName,
                    Year = b.Year,
                    Description = b.Description,
                    //Category = b.Category,
                    Position = b.Position,
                })
                .ToListAsync();
            return View(bikeVMs); // Trả về View Default.cshtml
        }

    }
}
