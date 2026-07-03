using CleanArchitect.Application.DTOs.BikeShop;
using CleanArchitect.Application.Interfaces;
using CleanArchitect.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitect.Infrastructure.Services
{
    public class CategoryGetAllService : ICategoryGetAll
    {
        private readonly EBikeShopDbContext _context;
        public CategoryGetAllService(EBikeShopDbContext context)
        {
            _context = context;
        }

        public async Task<CategoryDTO[]> GetAll()
        {
            var cateDTOs = new CategoryDTO[] { };
            try
            {
                cateDTOs = await _context.Categories
                    .OrderByDescending(c => c.Position)
                    .Select(c => new CategoryDTO
                    {
                        Id = c.Id,
                        Name = c.Position + ". " + c.Name
                    })
                    .ToArrayAsync();
            }
            catch (Exception ex)
            { }
            return cateDTOs;
        }
    }
}
