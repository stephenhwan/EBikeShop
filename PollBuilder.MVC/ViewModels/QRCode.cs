using Microsoft.AspNetCore.Mvc;

namespace PollBuilder.MVC.ViewModels
{
	[Bind("Text")]
	public class QRCodeVM
	{
		public string Text { get; set; } = string.Empty;
		public string? Url { get; set; }
		public string? ImagePath { get; set; }
	}
}
