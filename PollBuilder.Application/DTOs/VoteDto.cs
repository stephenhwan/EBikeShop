using PollBuilder.Domain.Entities;

namespace PollBuilder.Application.DTOs
{
	public class VoteDto
	{
		public Guid Id { get; set; }
		public string? Opinion { get; set; }
		public bool RequiredText { get; set; }
		public DateTime CreatedAt { get; set; }
		public int Position { get; set; }
		public int VoterToken { get; set; }


		//key
		public Guid UserId { get; set; }
		public virtual User User { get; set; }
		public Guid OptionId { get; set; }
		public virtual Option Option { get; set; }
		public Guid QuestionId { get; set; }
		public virtual Question Question { get; set; }
	}
}
