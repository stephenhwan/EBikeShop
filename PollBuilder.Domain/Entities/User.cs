using System;
using System.Collections.Generic;
using System.Text;

namespace PollBuilder.Domain.Entities
{
	public class User
	{
		public Guid Id { get; set; }
		public String UserName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public DateTime CreatedAt { get; set; }
	}

}
