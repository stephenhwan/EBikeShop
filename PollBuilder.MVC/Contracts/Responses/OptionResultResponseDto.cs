namespace PollBuilder.MVC.Contracts.Responses
{
	public class OptionResultResponseDto
	{
		public Guid Id { get; set; }
		public int Position { get; set; }
		public string OptionText { get; set; } = default!;
		public bool IsCurrent { get; set; }
	}
}
