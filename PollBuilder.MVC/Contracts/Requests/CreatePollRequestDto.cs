namespace PollBuilder.MVC.Contracts.Requests
{




	public class CreatePollRequestDto
	{
		public string Title { get; set; } = default!;
		public DateTime StartAt { get; set; }
		public DateTime EndAt { get; set; }
		public List<CreateQuestionRequestDto> Questions { get; set; } = new();
	}
}
