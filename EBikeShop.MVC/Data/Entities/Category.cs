using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace EBikeShop.MVC.Data.Entities
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(150, ErrorMessage = " Không được nhập quá 150 ký tự nha mấy cha nội")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(5000)]
        public string? Description { get; set; }

        [Range(1, int.MaxValue)]
        public int Position { get; set; }

        public virtual ICollection<Bike> Bikes { get; set; } = new Collection<Bike>();
    }
}
