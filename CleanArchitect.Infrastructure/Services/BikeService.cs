using CleanArchitect.Application.DTOs.BikeShop;
using CleanArchitect.Application.Interfaces;
using CleanArchitect.Domain.Entities.BikeShop;
using CleanArchitect.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitect.Infrastructure.Services
{
    public class BikeService : IBikeService
    {
        private readonly EBikeShopDbContext _context;
        public BikeService(EBikeShopDbContext context)
        {
            _context = context;
        }
        public async Task<Bike> Create(BikeDTO bikeDTO)
        {
            var countBikes = await _context.Bikes.CountAsync();
            var bike = new Bike
            {
                Name = bikeDTO.Name.Trim(),
                BrandName = bikeDTO.BrandName.Trim(),
                CategoryId = bikeDTO.CategoryId,
                //Category = bikeDTO.Category.Trim(),
                Description = bikeDTO.Description?.Trim(),
                ImageName = bikeDTO.ImagePath,
                Year = bikeDTO.Year,
                Position = ++countBikes
            };
            _context.Bikes.Add(bike);
            return bike;

        }

        public bool Delete(Guid idBike)
        {
            throw new NotImplementedException();
        }

        public List<Bike> GetAll()
        {
            throw new NotImplementedException();
        }

        public Bike GetById(Guid idBike)
        {
            throw new NotImplementedException();
        }

        public List<Bike> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public bool Update(BikeDTO bikeDTO)
        {
            throw new NotImplementedException();
        }
    }
}
