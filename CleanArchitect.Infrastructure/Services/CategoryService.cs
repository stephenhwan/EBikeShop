using CleanArchitect.Application.DTOs.BikeShop;
using CleanArchitect.Application.Interfaces;
using CleanArchitect.Domain.Entities.BikeShop;
using CleanArchitect.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitect.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly EBikeShopDbContext _context;
        public CategoryService(EBikeShopDbContext context)
        {
            _context = context;
        }

        public async Task<Category?> Create(CategoryDTO categoryDTO)
        {
            try
            {
                var countCategory = await _context.Categories.CountAsync();
                //category.Position = ++countCategory;
                //_context.Add(category);
                var newCategory = new Category
                {
                    //Id = Guid.NewGuid(),
                    Name = categoryDTO.Name.Trim(),
                    Description = categoryDTO.Description?.Trim(),
                    Position = ++countCategory
                };
                _context.Categories.Add(newCategory);
                await _context.SaveChangesAsync();
                return newCategory;
            }
            catch (Exception)
            {
            }
            return null;
        }

        public bool Delete(Guid idCategory)
        {
            throw new NotImplementedException();
        }

        public List<Category> GetAll()
        {
            throw new NotImplementedException();
        }

        public Category GetById(Guid idCategory)
        {
            throw new NotImplementedException();
        }

        public List<Category> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public bool Update(CategoryDTO categoryDTO)
        {
            throw new NotImplementedException();
        }
    }
}
