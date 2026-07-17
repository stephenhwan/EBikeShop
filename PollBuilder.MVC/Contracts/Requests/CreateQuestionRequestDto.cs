namespace PollBuilder.MVC.Contracts.Requests
{
	public class CreateQuestionRequestDto
	{
		public string QuestionText { get; set; } = default!;
		public int Position { get; set; }
		public List<CreateOptionRequestDto> Options { get; set; } = new();
	}
}
