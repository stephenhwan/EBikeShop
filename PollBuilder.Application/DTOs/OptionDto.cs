namespace PollBuilder.Application.DTOs
{
	public class OptionDto

	{
		public Guid Id { get; set; }
		public string OptionText { get; set; }
		public int Position { get; set; }

		//key
		public Guid QuestionId { get; set; }
		public virtual QuestionDto Question { get; set; }
	}
}
