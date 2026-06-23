using CleanArchitect.Application.DTOs.BikeShop;
using CleanArchitect.Domain.Entities.BikeShop;

namespace CleanArchitect.Application.Interfaces
{
    public interface ICategoryService
    {
        Category GetById(Guid idCategory);
        List<Category> GetAll();
        List<Category> GetByName(string name);

        Task<Category?> Create(CategoryDTO categoryDTO);
        bool Update(CategoryDTO categoryDTO);
        bool Delete(Guid idCategory);
    }
}
