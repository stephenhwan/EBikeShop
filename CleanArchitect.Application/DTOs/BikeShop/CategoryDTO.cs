namespace CleanArchitect.Application.DTOs.BikeShop
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int Position { get; set; }
    }
}
