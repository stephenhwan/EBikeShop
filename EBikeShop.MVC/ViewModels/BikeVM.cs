using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EBikeShop.MVC.ViewModels
{
    [Bind("Id,Name,Description,Year,BrandName,Category")]
    public class BikeVM
    {
        public Guid Id { get; set; }

        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(5000)]
        public string? Description { get; set; }

        public int Position { get; set; }

        [Range(1200, 9999)]
        public int Year { get; set; }

        [MaxLength(150)]
        public string BrandName { get; set; } = string.Empty;
        //public string? Image { get; set; }

        [MaxLength(150)]
        public string Category { get; set; } = string.Empty;
    }
}
