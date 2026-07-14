
using Microsoft.AspNetCore.Identity;

namespace PollBuilder.Domain.Entities.Identity
{
	public class UserRole : IdentityRole
	{
	 public string? Description { get; set; }
	}
}
