using PollBuilder.Domain.Entities;

namespace PollBuilder.Application.DTOs
{
	public class PollDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Url { get; set; }
		public bool Status { get; set; }
		public List<QuestionDto> Questions { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime? ClosedAt { get; set; }

		public DateTime? StartAt { get; set; }

		public DateTime? EndAt { get; set; }

		//key
		public Guid? UserId { get; set; }
		public virtual UserDto User { get; set; }
	}
}
