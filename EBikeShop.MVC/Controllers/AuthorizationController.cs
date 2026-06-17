using EBikeShop.MVC.Data.Entities.Identity;
using EBikeShop.MVC.ViewModels.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace EBikeShop.MVC.Controllers
{
	public class AuthorizationController : Controller
	{
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<BikeIdentityUser> _userManager;


		public AuthorizationController(UserManager<BikeIdentityUser> userManager,
									   RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}
		public async Task<IActionResult> AssignRole(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null) return NotFound();

			var assignRole = new AssignRoleVM
			{
				UserId = user.Id,
				UserEmail = user.Email,
				AllRoles = _roleManager.Roles.Select(r => r.Name).ToList(),
				UserRoles = (await _userManager.GetRolesAsync(user)).ToList()
			};
			return View(assignRole);
		}

		// Xóa role khỏi user
		public async Task<IActionResult> RemoveRole(string userId, string role)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null) return NotFound();

			await _userManager.RemoveFromRoleAsync(user, role);
			return RedirectToAction("Index");
		}
		[HttpPost]
		public async Task<IActionResult> AssignRole(AssignRoleVM vm)
		{
			var user = await _userManager.FindByIdAsync(vm.UserId);
			if (user == null) return NotFound();

			// Xóa hết role cũ rồi gán lại
			var currentRoles = await _userManager.GetRolesAsync(user);
			await _userManager.RemoveFromRolesAsync(user, currentRoles);
			await _userManager.AddToRolesAsync(user, vm.SelectedRoles);

			return RedirectToAction("Index");
		}

	}

}