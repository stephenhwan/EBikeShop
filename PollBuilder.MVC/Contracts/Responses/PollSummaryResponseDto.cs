namespace PollBuilder.MVC.Contracts.Responses
{
	public class PollSummaryResponseDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; } = default!;
		public string Url { get; set; } = default!;
		public bool Status { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
