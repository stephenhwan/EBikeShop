using Microsoft.AspNetCore.Identity;

namespace PollBuilder.Domain.Entities.Identity
{
	public class User : IdentityUser
	{
		public String UserName { get; set; }
		public string Email { get; set; }

		public DateTime CreatedAt { get; set; }
	}

}
