namespace PollBuilder.MVC.Contracts.Requests
{
	public class CreateOptionRequestDto
	{
		public string OptionText { get; set; } = default!;
		public int Position { get; set; }
	}

	public class CreateQuestionRequestDto
	{
		public string QuestionText { get; set; } = default!;
		public int Position { get; set; }
		public List<CreateOptionRequestDto> Options { get; set; } = new();
	}

	public class CreatePollRequestDto
	{
		public string Title { get; set; } = default!;
		public DateTime StartAt { get; set; }
		public DateTime EndAt { get; set; }
		public List<CreateQuestionRequestDto> Questions { get; set; } = new();
	}
}
