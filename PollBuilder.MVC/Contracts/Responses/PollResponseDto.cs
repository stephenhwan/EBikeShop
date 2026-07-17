namespace PollBuilder.MVC.Contracts.Responses
{
	public class OptionResponseDto
	{
		public Guid Id { get; set; }
		public string OptionText { get; set; } = default!;
		public int Position { get; set; }
	}

	public class QuestionResponseDto
	{
		public Guid Id { get; set; }
		public string QuestionText { get; set; } = default!;
		public int Position { get; set; }
		public List<OptionResponseDto> Options { get; set; } = new();
	}

	public class PollResponseDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; } = default!;
		public string Url { get; set; } = default!;
		public bool Status { get; set; }
		public List<QuestionResponseDto> Questions { get; set; } = new();
	}
}
