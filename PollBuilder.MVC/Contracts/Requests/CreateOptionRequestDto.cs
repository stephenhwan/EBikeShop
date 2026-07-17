namespace PollBuilder.MVC.Contracts.Requests
{
	public class CreateOptionRequestDto
	{
		public string OptionText { get; set; } = default!;
		public int Position { get; set; }
	}
}
