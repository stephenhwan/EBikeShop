using EBikeShop.MVC.Data;
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
            var bikes = await _dbContext.Bikes
                .ToListAsync();
            return View(bikes); // Trả về View Default.cshtml
        }

    }
}
