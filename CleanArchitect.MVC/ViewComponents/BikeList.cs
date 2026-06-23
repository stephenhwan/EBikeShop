using CleanArchitect.Infrastructure.DbContexts;
using CleanArchitect.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitect.MVC.ViewComponents
{
    public class BikeList : ViewComponent
    {
        private readonly EBikeShopDbContext _dbContext;
        public BikeList(EBikeShopDbContext dbContext)
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
