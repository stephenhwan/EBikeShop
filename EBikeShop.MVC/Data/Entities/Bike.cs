using System.ComponentModel.DataAnnotations;

namespace EBikeShop.MVC.Data.Entities
{

    public class Bike
    {
        public Bike()
        {
            Id = Guid.NewGuid();
            //Name = string.Empty;
        }
        public Guid Id { get; set; }

        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;


        [MaxLength(5000)]
        public string? Description { get; set; }

        [Range(1200, 9999)]
        public int Year { get; set; }

        [MaxLength(150)]
        public string BrandName { get; set; } = string.Empty;
		public int Position { get; set; }

		[MaxLength(150)]
		public string? CategoryName { get; set; }

		public Guid? CategoryId { get; set; }
		public Category? Category { get; set; }

		public ICollection<BikeMedia> BikeMedias { get; set; } = new List<BikeMedia>();

	}
}
