namespace EBikeShop.MVC.ViewModels.Identity
{
	public class AssignRoleVM
	{
		public string? UserId { get; set; }
		public string? UserEmail { get; set; }

		public List<string> AllRoles { get; set; } = new();

		public List<string> UserRoles { get; set; } = new();       // role user đang có
		public List<string> SelectedRoles { get; set; } = new();
	}
}
