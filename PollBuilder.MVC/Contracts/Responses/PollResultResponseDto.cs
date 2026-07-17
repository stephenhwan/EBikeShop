namespace PollBuilder.MVC.Contracts.Responses
{
	public class PollResultResponseDto
	{
		public string Url { get; set; } = default!;
		public List<QuestionResultResponseDto> Questions { get; set; } = new();
		public int VoteCount { get; set; }

	}
}
