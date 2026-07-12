namespace PollBuilder.Application.DTOs
{
	public class UserDto
	{
		public Guid Id { get; set; }
		public String UserName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
