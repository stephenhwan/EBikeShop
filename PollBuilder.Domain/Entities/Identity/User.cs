using Microsoft.AspNetCore.Identity;

namespace PollBuilder.Domain.Entities.Identity
{
	public class User : IdentityUser
	{
		public String? FullName { get; set; }
		public DateTime? CreatedAt { get; set; }


	}

}
