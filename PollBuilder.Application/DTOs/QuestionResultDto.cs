namespace PollBuilder.Application.DTOs
{
	public class QuestionResultDto
	{
		public Guid Id { get; set; }

		public string QuestionText { get; set; }

		public List<OptionResultDto> Options { get; set; }
	}
}
