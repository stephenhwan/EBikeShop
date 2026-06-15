using Microsoft.AspNetCore.Identity;
namespace EBikeShop.MVC.Data.Entities.Identity
{
	public class BikeIdentityUser : IdentityUser
	{
		public string? FullName { get; set; }
		public string? DisplayName { get; set; }
		public string? Avatar { get; set;  }
	}
}
