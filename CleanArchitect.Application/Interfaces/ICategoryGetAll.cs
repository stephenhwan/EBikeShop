using CleanArchitect.Application.DTOs.BikeShop;

namespace CleanArchitect.Application.Interfaces
{
    public interface ICategoryGetAll
    {
        Task<CategoryDTO[]> GetAll();
    }
}
