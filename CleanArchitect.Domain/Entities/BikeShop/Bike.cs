namespace CleanArchitect.Domain.Entities.BikeShop
{
    public class Bike
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        //[MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        //[MaxLength(255)]
        public string? ImageName { get; set; }

        //[MaxLength(5000)]
        public string? Description { get; set; }

        public int Position { get; set; }

        //[Range(1200, 9999)]
        public int Year { get; set; }

        //[MaxLength(150)]
        public string BrandName { get; set; } = string.Empty;
        //public string? Image { get; set; }

        //[MaxLength(150)]
        //public string Category222 { get; set; } = string.Empty;

        public Guid? CategoryId { get; set; }
        public virtual Category Category { get; set; }

    }
}
