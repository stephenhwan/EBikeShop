using CleanArchitect.Application.DTOs.BikeShop;
using CleanArchitect.Domain.Entities.BikeShop;

namespace CleanArchitect.Application.Interfaces
{
    public interface IBikeService
    {
        Bike GetById(Guid idBike);
        List<Bike> GetAll();
        List<Bike> GetByName(string name);

        Task<Bike> Create(BikeDTO bikeDTO);
        bool Update(BikeDTO bikeDTO);
        bool Delete(Guid idBike);

    }
}
