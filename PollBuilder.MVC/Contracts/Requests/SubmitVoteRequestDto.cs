namespace PollBuilder.MVC.Contracts.Requests
{
	public class SubmitVoteRequestDto
	{
		public List<Guid> SelectedOptionIds { get; set; } = new();
	}
}
