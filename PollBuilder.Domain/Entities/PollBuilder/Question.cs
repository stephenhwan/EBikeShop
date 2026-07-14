
namespace PollBuilder.Domain.Entities.PollBuilder
{
	public class Question
	{
		public Guid Id { get; set; }
		public string QuestionText { get; set; }
		public int Position { get; set; }
		public ICollection<Option> Options { get; set; } = new List<Option>();

		//key
		public Guid PollId { get; set; }
		public virtual Poll Poll { get; set; }
	}
}
