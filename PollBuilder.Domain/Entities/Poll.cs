using PollBuilder.Common.Contants;
namespace PollBuilder.Domain.Entities
{
	public class Poll
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Url { get; set; }
		public string Status { get;set; }
		public ICollection<Question> Questions { get; set; } = new List<Question>();
		public DateTime CreatedAt { get; set; }
		public DateTime? ClosedAt { get; set; }

		//key
		public Guid? UserId { get; set; }
		public virtual User User { get; set; }
		public bool IsClosed { get; private set; }

		//behavior
		public void Close()
		{
			if (Status == PollStatus.Closed) throw new InvalidOperationException("Poll này đã đóng rồi.");
			Status = PollStatus.Closed;
			ClosedAt = DateTime.UtcNow;

		}
	}
}
