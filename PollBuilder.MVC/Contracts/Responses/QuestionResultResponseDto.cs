namespace PollBuilder.MVC.Contracts.Responses
{
	public class QuestionResultResponseDto
	{
		public Guid Id { get; set; }
		public string QuestionText { get; set; } = default!;
		public List<OptionResultResponseDto> Options { get; set; } = new();
	}
}
