using PollBuilder.Common.Contants;
using PollBuilder.Domain.Entities.Identity;
namespace PollBuilder.Domain.Entities.PollBuilder
{
	public class Poll
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Url { get; set; }
		public bool Status { get; set; }
		public ICollection<Question> Questions { get; set; } = new List<Question>();
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public DateTime? ClosedAt { get; set; }
		public DateTime StartAt { get; set; }
		public DateTime EndAt { get; set; }
		public string UserId { get; set; }
		// computed property
		public bool IsOpen => Status
			&& StartAt <= DateTime.UtcNow
			&& EndAt >= DateTime.UtcNow;

		//key


		//behavior
		public void Close()
		{
			if (Status == false) throw new InvalidOperationException("Poll này đã đóng rồi.");
			Status = false;
			ClosedAt = DateTime.UtcNow;
		}

		public void Schedule(DateTime startAt, DateTime endAt)
		{
			if (startAt >= endAt) throw new InvalidOperationException("StartAt phải trước EndAt.");
			StartAt = startAt;
			EndAt = endAt;
		}
	}
}
	