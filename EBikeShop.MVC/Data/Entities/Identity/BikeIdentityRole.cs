using Microsoft.AspNetCore.Identity;
namespace EBikeShop.MVC.Data.Entities.Identity
{
	public class BikeIdentityRole : IdentityRole
	{
		public string? Description { get; set; }
	}
}
