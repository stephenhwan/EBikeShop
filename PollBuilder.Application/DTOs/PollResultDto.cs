namespace PollBuilder.Application.DTOs
{
	public class PollResultDto
	{
		public string TitlePoll { get; set; }
		public string Url { get; set; }
		public List<QuestionResultDto>  Questions { get; set; }
		public bool IsCurrent { get; set; } = false;
		public int VoteCount { get; set; }


	}
}
