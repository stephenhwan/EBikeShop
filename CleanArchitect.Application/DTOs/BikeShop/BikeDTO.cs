namespace CleanArchitect.Application.DTOs.BikeShop
{
    public class BikeDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        //public IFormFile? Image { get; set; }
        public string? ImagePath { get; set; }

        public int Position { get; set; }

        public int Year { get; set; }

        public string BrandName { get; set; } = string.Empty;

        public Guid? CategoryId { get; set; }
    }
}
